// Copyright (c) 2013-2016 Cemalettin Dervis, MIT License.
// https://github.com/cemdervis/SharpConfig

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace SharpConfig
{
    /// <summary>
    /// Represents a group of <see cref="Setting"/> objects.
    /// </summary>
    public sealed class Section : ConfigurationElement, IEnumerable<Setting>
    {
        private readonly List<Setting> mSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        ///
        /// <param name="name">The name of the section.</param>
        public Section(string name)
            : base(name)
        {
            mSettings = new List<Setting>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Section"/> class that is
        /// based on an existing object.
        /// Important: the section is built only from the public getter properties
        /// and fields of its type.
        /// When this method is called, all of those properties will be called
        /// and fields accessed once to obtain their values.
        /// Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute
        /// or are of a type that is marked with that attribute, are ignored.
        /// </summary>
        /// <param name="name">The name of the section.</param>
        /// <param name="obj"></param>
        /// <returns>The newly created section.</returns>
        /// 
        /// <exception cref="ArgumentException">When <paramref name="name"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="obj"/> is null.</exception>
        public static Section FromObject(string name, object obj)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The section name must not be null or empty.", "name");

            if (obj == null)
                throw new ArgumentNullException("obj");

            var section = new Section(name);
            var type = obj.GetType();

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanRead || ShouldIgnoreMappingFor(prop))
                {
                    // Skip this property, as it can't be read from.
                    continue;
                }

                var setting = new Setting(prop.Name, prop.GetValue(obj, null));
                section.mSettings.Add(setting);
            }

            // Repeat for each public field.
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (ShouldIgnoreMappingFor(field))
                {
                    // Skip this field.
                    continue;
                }

                var setting = new Setting(field.Name, field.GetValue(obj));
                section.mSettings.Add(setting);
            }

            return section;
        }

        /// <summary>
        /// Creates an object of a specific type, and maps the settings
        /// in this section to the public properties and writable fields of the object.
        /// Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute
        /// or are of a type that is marked with that attribute, are ignored.
        /// </summary>
        /// 
        /// <typeparam name="T">
        /// The type of object to create.
        /// Note: the type must be default-constructible, meaning it has a public default constructor.
        /// </typeparam>
        /// 
        /// <returns>The created object.</returns>
        /// 
        /// <remarks>
        /// The specified type must have a public default constructor
        /// in order to be created.
        /// </remarks>
        public T ToObject<T>() where T : new()
        {
            var obj = Activator.CreateInstance<T>();
            SetValuesTo(obj);
            return obj;
        }

        /// <summary>
        /// Creates an object of a specific type, and maps the settings
        /// in this section to the public properties and writable fields of the object.
        /// Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute
        /// or are of a type that is marked with that attribute, are ignored.
        /// </summary>
        /// 
        /// <param name="type">
        /// The type of object to create.
        /// Note: the type must be default-constructible, meaning it has a public default constructor.
        /// </param>
        /// 
        /// <returns>The created object.</returns>
        /// 
        /// <remarks>
        /// The specified type must have a public default constructor
        /// in order to be created.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentNullException">When <paramref name="type"/> is null.</exception>
        public object ToObject(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(type.Name);

            var obj = Activator.CreateInstance(type);
            SetValuesTo(obj);
            return obj;
        }

        /// <summary>
        /// Assigns the values of an object's public properties and fields to the corresponding
        /// <b>already existing</b> settings in this section.
        /// Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute
        /// or are of a type that is marked with that attribute, are ignored.
        /// </summary>
        /// 
        /// <param name="obj">The object from which the values are obtained.</param>
        /// 
        /// <exception cref="ArgumentNullException">When <paramref name="obj"/> is null.</exception>
        public void GetValuesFrom(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var type = obj.GetType();

            // Scan the type's properties.
            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanRead || ShouldIgnoreMappingFor(prop))
                    continue;

                var setting = FindSetting(prop.Name);
                if (setting != null)
                {
                    object value = prop.GetValue(obj, null);
                    setting.SetValue(value);
                }
            }

            // Scan the type's fields.
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (ShouldIgnoreMappingFor(field))
                    continue;

                var setting = FindSetting(field.Name);
                if (setting != null)
                {
                    object value = field.GetValue(obj);
                    setting.SetValue(value);
                }
            }
        }

        /// <summary>
        /// Assigns the values of this section to an object's public properties and fields.
        /// Properties and fields that are marked with the <see cref="IgnoreAttribute"/> attribute
        /// or are of a type that is marked with that attribute, are ignored.
        /// </summary>
        /// 
        /// <param name="obj">The object that is modified based on the section.</param>
        /// 
        /// <exception cref="ArgumentNullException">When <paramref name="obj"/> is null.</exception>
        public void SetValuesTo(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var type = obj.GetType();

            // Scan the type's properties.
            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanWrite || ShouldIgnoreMappingFor(prop))
                    continue;

                var setting = FindSetting(prop.Name);
                if (setting == null)
                    continue;

                object value = setting.GetValue(prop.PropertyType);
                if (value is Array)
                {
                    var settingArray = value as Array;
                    var propArray = prop.GetValue(obj, null) as Array;
                    if (propArray == null || propArray.Length != settingArray.Length)
                    {
                        // (Re)create the property's array.
                        propArray = Array.CreateInstance(prop.PropertyType.GetElementType(), settingArray.Length);
                    }

                    for (int i = 0; i < settingArray.Length; i++)
                        propArray.SetValue(settingArray.GetValue(i), i);

                    prop.SetValue(obj, propArray, null);
                }
                else
                {
                    prop.SetValue(obj, value, null);
                }
            }

            // Scan the type's fields.
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                // Skip readonly fields.
                if (field.IsInitOnly || ShouldIgnoreMappingFor(field))
                    continue;

                var setting = FindSetting(field.Name);
                if (setting == null)
                    continue;

                object value = setting.GetValue(field.FieldType);
                if (value is Array)
                {
                    var settingArray = value as Array;
                    var fieldArray = field.GetValue(obj) as Array;
                    if (fieldArray == null || fieldArray.Length != settingArray.Length)
                    {
                        // (Re)create the field's array.
                        fieldArray = Array.CreateInstance(field.FieldType.GetElementType(), settingArray.Length);
                    }

                    for (int i = 0; i < settingArray.Length; i++)
                        fieldArray.SetValue(settingArray.GetValue(i), i);

                    field.SetValue(obj, fieldArray);
                }
                else
                {
                    field.SetValue(obj, value);
                }
            }
        }

        // Determines whether a member should be ignored.
        private static bool ShouldIgnoreMappingFor(MemberInfo member)
        {
            if (member.GetCustomAttributes(typeof(IgnoreAttribute), false).Length > 0)
            {
                return true;
            }
            else
            {
                var prop = member as PropertyInfo;
                if (prop != null)
                    return prop.PropertyType.GetCustomAttributes(typeof(IgnoreAttribute), false).Length > 0;

                var field = member as FieldInfo;
                if (field != null)
                    return field.FieldType.GetCustomAttributes(typeof(IgnoreAttribute), false).Length > 0;
            }

            return false;
        }

        /// <summary>
        /// Gets an enumerator that iterates through the section.
        /// </summary>
        public IEnumerator<Setting> GetEnumerator()
        {
            return mSettings.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that iterates through the section.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a setting to the section.
        /// </summary>
        /// <param name="setting">The setting to add.</param>
        /// 
        /// <exception cref="ArgumentNullException">When <paramref name="setting"/> is null.</exception>
        /// <exception cref="ArgumentException">When the specified setting already exists in the section.</exception>
        public void Add(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            if (Contains(setting))
                throw new ArgumentException("The specified setting already exists in the section.");
            
            mSettings.Add(setting);
        }

        /// <summary>
        /// Removes a setting from the section by its name.
        /// If there are multiple settings with the same name, only the first setting is removed.
        /// To remove all settings that have the name name, use the RemoveAllNamed() method instead.
        /// </summary>
        /// <param name="settingName">The case-sensitive name of the setting to remove.</param>
        /// <returns>True if a setting with the specified name was removed; false otherwise.</returns>
        /// 
        /// <exception cref="ArgumentNullException">When <paramref name="settingName"/> is null or empty.</exception>
        public bool Remove(string settingName)
        {
            if (string.IsNullOrEmpty(settingName))
                throw new ArgumentNullException("settingName");

            return Remove(FindSetting(settingName));
        }

        /// <summary>
        /// Removes a setting from the section.
        /// </summary>
        /// <param name="setting">The setting to remove.</param>
        /// <returns>True if the setting was removed; false otherwise.</returns>
        public bool Remove(Setting setting)
        {
            return mSettings.Remove(setting);
        }

        /// <summary>
        /// Removes all settings that have a specific name.
        /// </summary>
        /// <param name="settingName">The case-sensitive name of the settings to remove.</param>
        /// 
        /// <exception cref="ArgumentNullException">When <paramref name="settingName"/> is null or empty.</exception>
        public void RemoveAllNamed(string settingName)
        {
            if (string.IsNullOrEmpty(settingName))
                throw new ArgumentNullException("settingName");

            while (Remove(settingName)) ;
        }

        /// <summary>
        /// Clears the section of all settings.
        /// </summary>
        public void Clear()
        {
            mSettings.Clear();
        }

        /// <summary>
        /// Determines whether a specified setting is contained in the section.
        /// </summary>
        /// <param name="setting">The setting to check for containment.</param>
        /// <returns>True if the setting is contained in the section; false otherwise.</returns>
        public bool Contains(Setting setting)
        {
            return mSettings.Contains(setting);
        }

        /// <summary>
        /// Determines whether a specifically named setting is contained in the section.
        /// </summary>
        /// <param name="settingName">The case-sensitive name of the setting.</param>
        /// <returns>True if the setting is contained in the section; false otherwise.</returns>
        ///
        /// <exception cref="ArgumentNullException">When <paramref name="settingName"/> is null or empty.</exception>
        public bool Contains(string settingName)
        {
            if (string.IsNullOrEmpty(settingName))
                throw new ArgumentNullException("settingName");

            return FindSetting(settingName) != null;
        }

        /// <summary>
        /// Gets the number of settings that are in the section.
        /// </summary>
        public int SettingCount
        {
            get { return mSettings.Count; }
        }

        /// <summary>
        /// Gets or sets a setting by index.
        /// </summary>
        /// <param name="index">The index of the setting in the section.</param>
        /// 
        /// <returns>
        /// The setting at the specified index.
        /// Note: no setting is created when using this accessor.
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="index"/> is out of range.</exception>
        public Setting this[int index]
        {
            get
            {
                if (index < 0 || index >= mSettings.Count)
                    throw new ArgumentOutOfRangeException("index");

                return mSettings[index];
            }
        }

        /// <summary>
        /// Gets or sets a setting by its name.
        /// If there are multiple settings with the same name, the first setting is returned.
        /// If you want to obtain all settings that have the same name, use the GetSettingsNamed() method instead.
        /// </summary>
        ///
        /// <param name="name">The case-sensitive name of the setting.</param>
        ///
        /// <returns>
        /// The setting if found, otherwise a new setting with
        /// the specified name is created, added to the section and returned.
        /// </returns>
        public Setting this[string name]
        {
            get
            {
                var setting = FindSetting(name);
                if (setting == null)
                {
                    setting = new Setting(name);
                    mSettings.Add(setting);
                }

                return setting;
            }
        }

        /// <summary>
        /// Gets all settings that have a specific name.
        /// </summary>
        /// <param name="name">The case-sensitive name of the settings.</param>
        /// <returns>
        /// The found settings.
        /// </returns>
        public IEnumerable<Setting> GetSettingsNamed(string name)
        {
            var settings = new List<Setting>();

            foreach (var setting in mSettings)
            {
                if (string.Equals(setting.Name, name, StringComparison.OrdinalIgnoreCase))
                    settings.Add(setting);
            }

            return settings;
        }

        // Finds a setting by its name.
        private Setting FindSetting(string name)
        {
            foreach (var setting in mSettings)
            {
                if (string.Equals(setting.Name, name, StringComparison.OrdinalIgnoreCase))
                    return setting;
            }

            return null;
        }

        /// <summary>
        /// Gets the element's expression as a string.
        /// An example for a section would be "[Section]".
        /// </summary>
        /// <returns>The element's expression as a string.</returns>
        protected override string GetStringExpression()
        {
            return string.Format("[{0}]", Name);
        }
    }
}
