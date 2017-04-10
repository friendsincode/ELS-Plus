using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using CitizenFX.Core;

namespace ELS.configuration
{
    public class VCF
    {
        public static List<vcfroot> ELSVehicle = new List<vcfroot>();
        public VCF()
        {
        }
        public static void load(SettingsType.Type type, string name,string Data)
        {

            var data = new vcfroot();
            data.FileName = Path.GetFileNameWithoutExtension(name);
            Debug.WriteLine();
            data.SOUNDS = new vcfrootSOUNDS();
            data.SOUNDS.MainHorn = new vcfrootSOUNDSMainHorn();

            data.SOUNDS.SrnTone1 = new vcfrootSOUNDSSrnTone1();
            data.SOUNDS.SrnTone2 = new vcfrootSOUNDSSrnTone2();
            data.SOUNDS.SrnTone3 = new vcfrootSOUNDSSrnTone3();
            data.SOUNDS.SrnTone4 = new vcfrootSOUNDSSrnTone4();
            data.SOUNDS.AuxSiren = new vcfrootSOUNDSAuxSiren();
            data.SOUNDS.ManTone1 = new vcfrootSOUNDSManTone1();
            data.SOUNDS.ManTone2 = new vcfrootSOUNDSManTone2();
            data.SOUNDS.PanicMde = new vcfrootSOUNDSPanicMde();

            data.ACORONAS = new vcfrootACORONAS();

            data.CRUISE = new vcfrootCRUISE();

            data.EOVERRIDE = new vcfrootEOVERRIDE();
            data.EOVERRIDE.Extra01 = new vcfrootEOVERRIDEExtra01();
            data.EOVERRIDE.Extra02 = new vcfrootEOVERRIDEExtra02();
            data.EOVERRIDE.Extra03 = new vcfrootEOVERRIDEExtra03();
            data.EOVERRIDE.Extra04 = new vcfrootEOVERRIDEExtra04();
            data.EOVERRIDE.Extra05 = new vcfrootEOVERRIDEExtra05();
            data.EOVERRIDE.Extra06 = new vcfrootEOVERRIDEExtra06();
            data.EOVERRIDE.Extra07 = new vcfrootEOVERRIDEExtra07();
            data.EOVERRIDE.Extra08 = new vcfrootEOVERRIDEExtra08();
            data.EOVERRIDE.Extra09 = new vcfrootEOVERRIDEExtra09();
            data.EOVERRIDE.Extra10 = new vcfrootEOVERRIDEExtra10();
            data.EOVERRIDE.Extra11 = new vcfrootEOVERRIDEExtra11();
            data.EOVERRIDE.Extra12 = new vcfrootEOVERRIDEExtra12();

            data.INTERFACE = new vcfrootINTERFACE();

            data.MISC = new vcfrootMISC();

            data.PRML = new vcfrootPRML();

            data.SECL = new vcfrootSECL();

            data.WRNL = new vcfrootWRNL();
            if (type == SettingsType.Type.VCF)
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(Data);
                //Debug.WriteLine(doc.DocumentElement);
                bool res;

                data.SOUNDS.ManTone1.AudioString = doc["vcfroot"]["SOUNDS"]["ManTone1"].Attributes["AudioString"].Value;
                data.SOUNDS.ManTone1.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["ManTone1"].Attributes["AllowUse"].Value);

                data.SOUNDS.ManTone2.AudioString = doc["vcfroot"]["SOUNDS"]["ManTone2"].Attributes["AudioString"].Value;
                data.SOUNDS.ManTone2.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["ManTone2"].Attributes["AllowUse"].Value);


                data.SOUNDS.MainHorn.AudioString = doc["vcfroot"]["SOUNDS"]["MainHorn"].Attributes["AudioString"].Value;
                data.SOUNDS.MainHorn.InterruptsSiren = bool.Parse(doc["vcfroot"]["SOUNDS"]["MainHorn"].Attributes["InterruptsSiren"].Value);

                data.SOUNDS.SrnTone1.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone1"].Attributes["AudioString"].Value;
                data.SOUNDS.SrnTone1.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["SrnTone1"].Attributes["AllowUse"].Value);

                data.SOUNDS.SrnTone2.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone2"].Attributes["AudioString"].Value;
                data.SOUNDS.SrnTone2.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["SrnTone2"].Attributes["AllowUse"].Value);

                data.SOUNDS.SrnTone3.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone3"].Attributes["AudioString"].Value;
                data.SOUNDS.SrnTone3.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["SrnTone3"].Attributes["AllowUse"].Value);

                data.SOUNDS.SrnTone4.AudioString = doc["vcfroot"]["SOUNDS"]["SrnTone4"].Attributes["AudioString"].Value;
                data.SOUNDS.SrnTone4.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["SrnTone4"].Attributes["AllowUse"].Value);

                data.SOUNDS.AuxSiren.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["AuxSiren"].Attributes["AllowUse"].Value);
                data.SOUNDS.AuxSiren.AudioString = doc["vcfroot"]["SOUNDS"]["AuxSiren"].Attributes["AudioString"].Value;

                data.SOUNDS.PanicMde.AllowUse = bool.Parse(doc["vcfroot"]["SOUNDS"]["PanicMde"].Attributes["AllowUse"].Value);
                data.SOUNDS.PanicMde.AudioString = doc["vcfroot"]["SOUNDS"]["PanicMde"].Attributes["AudioString"].Value;

                data.Author = doc["vcfroot"].Attributes["Author"].Value;
                ELSVehicle.Add(data);
                foreach (var vcfroot in ELSVehicle)
                {
                    Debug.WriteLine(vcfroot.FileName);
                }
            }
        }
        /// <remarks/>

        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class vcfroot
        {

            private vcfrootINTERFACE iNTERFACEField;

            private vcfrootEOVERRIDE eOVERRIDEField;

            private vcfrootMISC mISCField;

            private vcfrootCRUISE cRUISEField;

            private vcfrootACORONAS aCORONASField;

            private vcfrootSOUNDS sOUNDSField;

            private vcfrootWRNL wRNLField;

            private vcfrootPRML pRMLField;

            private vcfrootSECL sECLField;

            private string descriptionField;

            private string authorField;

            private string FileNameField;
            /// <remarks/>
            public vcfrootINTERFACE INTERFACE
            {
                get
                {
                    return this.iNTERFACEField;
                }
                set
                {
                    this.iNTERFACEField = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDE EOVERRIDE
            {
                get
                {
                    return this.eOVERRIDEField;
                }
                set
                {
                    this.eOVERRIDEField = value;
                }
            }

            /// <remarks/>
            public vcfrootMISC MISC
            {
                get
                {
                    return this.mISCField;
                }
                set
                {
                    this.mISCField = value;
                }
            }

            /// <remarks/>
            public vcfrootCRUISE CRUISE
            {
                get
                {
                    return this.cRUISEField;
                }
                set
                {
                    this.cRUISEField = value;
                }
            }

            /// <remarks/>
            public vcfrootACORONAS ACORONAS
            {
                get
                {
                    return this.aCORONASField;
                }
                set
                {
                    this.aCORONASField = value;
                }
            }

            /// <remarks/>
            public vcfrootSOUNDS SOUNDS
            {
                get
                {
                    return this.sOUNDSField;
                }
                set
                {
                    this.sOUNDSField = value;
                }
            }

            /// <remarks/>
            public vcfrootWRNL WRNL
            {
                get
                {
                    return this.wRNLField;
                }
                set
                {
                    this.wRNLField = value;
                }
            }

            /// <remarks/>
            public vcfrootPRML PRML
            {
                get
                {
                    return this.pRMLField;
                }
                set
                {
                    this.pRMLField = value;
                }
            }

            /// <remarks/>
            public vcfrootSECL SECL
            {
                get
                {
                    return this.sECLField;
                }
                set
                {
                    this.sECLField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Author
            {
                get
                {
                    return this.authorField;
                }
                set
                {
                    this.authorField = value;
                }
            }

            public string FileName
            {
                get
                {
                    return this.FileNameField;
                }
                set
                {
                    this.FileNameField = value; 
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootINTERFACE
        {

            private string lstgActivationTypeField;

            private string defaultSirenModeField;

            private string infoPanelHeaderColorField;

            private string infoPanelButtonLightColorField;

            /// <remarks/>
            public string LstgActivationType
            {
                get
                {
                    return this.lstgActivationTypeField;
                }
                set
                {
                    this.lstgActivationTypeField = value;
                }
            }

            /// <remarks/>
            public string DefaultSirenMode
            {
                get
                {
                    return this.defaultSirenModeField;
                }
                set
                {
                    this.defaultSirenModeField = value;
                }
            }

            /// <remarks/>
            public string InfoPanelHeaderColor
            {
                get
                {
                    return this.infoPanelHeaderColorField;
                }
                set
                {
                    this.infoPanelHeaderColorField = value;
                }
            }

            /// <remarks/>
            public string InfoPanelButtonLightColor
            {
                get
                {
                    return this.infoPanelButtonLightColorField;
                }
                set
                {
                    this.infoPanelButtonLightColorField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDE
        {

            private vcfrootEOVERRIDEExtra01 extra01Field;

            private vcfrootEOVERRIDEExtra02 extra02Field;

            private vcfrootEOVERRIDEExtra03 extra03Field;

            private vcfrootEOVERRIDEExtra04 extra04Field;

            private vcfrootEOVERRIDEExtra05 extra05Field;

            private vcfrootEOVERRIDEExtra06 extra06Field;

            private vcfrootEOVERRIDEExtra07 extra07Field;

            private vcfrootEOVERRIDEExtra08 extra08Field;

            private vcfrootEOVERRIDEExtra09 extra09Field;

            private vcfrootEOVERRIDEExtra10 extra10Field;

            private vcfrootEOVERRIDEExtra11 extra11Field;

            private vcfrootEOVERRIDEExtra12 extra12Field;

            /// <remarks/>
            public vcfrootEOVERRIDEExtra01 Extra01
            {
                get
                {
                    return this.extra01Field;
                }
                set
                {
                    this.extra01Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra02 Extra02
            {
                get
                {
                    return this.extra02Field;
                }
                set
                {
                    this.extra02Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra03 Extra03
            {
                get
                {
                    return this.extra03Field;
                }
                set
                {
                    this.extra03Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra04 Extra04
            {
                get
                {
                    return this.extra04Field;
                }
                set
                {
                    this.extra04Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra05 Extra05
            {
                get
                {
                    return this.extra05Field;
                }
                set
                {
                    this.extra05Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra06 Extra06
            {
                get
                {
                    return this.extra06Field;
                }
                set
                {
                    this.extra06Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra07 Extra07
            {
                get
                {
                    return this.extra07Field;
                }
                set
                {
                    this.extra07Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra08 Extra08
            {
                get
                {
                    return this.extra08Field;
                }
                set
                {
                    this.extra08Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra09 Extra09
            {
                get
                {
                    return this.extra09Field;
                }
                set
                {
                    this.extra09Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra10 Extra10
            {
                get
                {
                    return this.extra10Field;
                }
                set
                {
                    this.extra10Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra11 Extra11
            {
                get
                {
                    return this.extra11Field;
                }
                set
                {
                    this.extra11Field = value;
                }
            }

            /// <remarks/>
            public vcfrootEOVERRIDEExtra12 Extra12
            {
                get
                {
                    return this.extra12Field;
                }
                set
                {
                    this.extra12Field = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra01
        {

            private bool isElsControlledField;

            private bool allowEnvLightField;

            private string colorField;

            private decimal offsetXField;

            private decimal offsetYField;

            private decimal offsetZField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowEnvLight
            {
                get
                {
                    return this.allowEnvLightField;
                }
                set
                {
                    this.allowEnvLightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetX
            {
                get
                {
                    return this.offsetXField;
                }
                set
                {
                    this.offsetXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetY
            {
                get
                {
                    return this.offsetYField;
                }
                set
                {
                    this.offsetYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetZ
            {
                get
                {
                    return this.offsetZField;
                }
                set
                {
                    this.offsetZField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra02
        {

            private bool isElsControlledField;

            private bool allowEnvLightField;

            private string colorField;

            private decimal offsetXField;

            private decimal offsetYField;

            private decimal offsetZField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowEnvLight
            {
                get
                {
                    return this.allowEnvLightField;
                }
                set
                {
                    this.allowEnvLightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetX
            {
                get
                {
                    return this.offsetXField;
                }
                set
                {
                    this.offsetXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetY
            {
                get
                {
                    return this.offsetYField;
                }
                set
                {
                    this.offsetYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetZ
            {
                get
                {
                    return this.offsetZField;
                }
                set
                {
                    this.offsetZField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra03
        {

            private bool isElsControlledField;

            private bool allowEnvLightField;

            private string colorField;

            private decimal offsetXField;

            private decimal offsetYField;

            private decimal offsetZField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowEnvLight
            {
                get
                {
                    return this.allowEnvLightField;
                }
                set
                {
                    this.allowEnvLightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetX
            {
                get
                {
                    return this.offsetXField;
                }
                set
                {
                    this.offsetXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetY
            {
                get
                {
                    return this.offsetYField;
                }
                set
                {
                    this.offsetYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetZ
            {
                get
                {
                    return this.offsetZField;
                }
                set
                {
                    this.offsetZField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra04
        {

            private bool isElsControlledField;

            private bool allowEnvLightField;

            private string colorField;

            private decimal offsetXField;

            private decimal offsetYField;

            private decimal offsetZField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowEnvLight
            {
                get
                {
                    return this.allowEnvLightField;
                }
                set
                {
                    this.allowEnvLightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetX
            {
                get
                {
                    return this.offsetXField;
                }
                set
                {
                    this.offsetXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetY
            {
                get
                {
                    return this.offsetYField;
                }
                set
                {
                    this.offsetYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetZ
            {
                get
                {
                    return this.offsetZField;
                }
                set
                {
                    this.offsetZField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra05
        {

            private bool isElsControlledField;

            private bool allowEnvLightField;

            private string colorField;

            private decimal offsetXField;

            private decimal offsetYField;

            private decimal offsetZField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowEnvLight
            {
                get
                {
                    return this.allowEnvLightField;
                }
                set
                {
                    this.allowEnvLightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetX
            {
                get
                {
                    return this.offsetXField;
                }
                set
                {
                    this.offsetXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetY
            {
                get
                {
                    return this.offsetYField;
                }
                set
                {
                    this.offsetYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetZ
            {
                get
                {
                    return this.offsetZField;
                }
                set
                {
                    this.offsetZField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra06
        {

            private bool isElsControlledField;

            private bool allowEnvLightField;

            private string colorField;

            private decimal offsetXField;

            private decimal offsetYField;

            private decimal offsetZField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowEnvLight
            {
                get
                {
                    return this.allowEnvLightField;
                }
                set
                {
                    this.allowEnvLightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetX
            {
                get
                {
                    return this.offsetXField;
                }
                set
                {
                    this.offsetXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetY
            {
                get
                {
                    return this.offsetYField;
                }
                set
                {
                    this.offsetYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetZ
            {
                get
                {
                    return this.offsetZField;
                }
                set
                {
                    this.offsetZField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra07
        {

            private bool isElsControlledField;

            private bool allowEnvLightField;

            private string colorField;

            private decimal offsetXField;

            private decimal offsetYField;

            private decimal offsetZField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowEnvLight
            {
                get
                {
                    return this.allowEnvLightField;
                }
                set
                {
                    this.allowEnvLightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetX
            {
                get
                {
                    return this.offsetXField;
                }
                set
                {
                    this.offsetXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetY
            {
                get
                {
                    return this.offsetYField;
                }
                set
                {
                    this.offsetYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetZ
            {
                get
                {
                    return this.offsetZField;
                }
                set
                {
                    this.offsetZField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra08
        {

            private bool isElsControlledField;

            private bool allowEnvLightField;

            private string colorField;

            private decimal offsetXField;

            private decimal offsetYField;

            private decimal offsetZField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowEnvLight
            {
                get
                {
                    return this.allowEnvLightField;
                }
                set
                {
                    this.allowEnvLightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetX
            {
                get
                {
                    return this.offsetXField;
                }
                set
                {
                    this.offsetXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetY
            {
                get
                {
                    return this.offsetYField;
                }
                set
                {
                    this.offsetYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetZ
            {
                get
                {
                    return this.offsetZField;
                }
                set
                {
                    this.offsetZField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra09
        {

            private bool isElsControlledField;

            private bool allowEnvLightField;

            private string colorField;

            private decimal offsetXField;

            private decimal offsetYField;

            private decimal offsetZField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowEnvLight
            {
                get
                {
                    return this.allowEnvLightField;
                }
                set
                {
                    this.allowEnvLightField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetX
            {
                get
                {
                    return this.offsetXField;
                }
                set
                {
                    this.offsetXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetY
            {
                get
                {
                    return this.offsetYField;
                }
                set
                {
                    this.offsetYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal OffsetZ
            {
                get
                {
                    return this.offsetZField;
                }
                set
                {
                    this.offsetZField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra10
        {

            private bool isElsControlledField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra11
        {

            private bool isElsControlledField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootEOVERRIDEExtra12
        {

            private bool isElsControlledField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IsElsControlled
            {
                get
                {
                    return this.isElsControlledField;
                }
                set
                {
                    this.isElsControlledField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootMISC
        {

            private bool vehicleIsSlicktopField;

            private string arrowboardTypeField;

            private bool useSteadyBurnLightsField;

            private byte dfltSirenLtsActivateAtLstgField;

            private vcfrootMISCTakedowns takedownsField;

            private vcfrootMISCSceneLights sceneLightsField;

            /// <remarks/>
            public bool VehicleIsSlicktop
            {
                get
                {
                    return this.vehicleIsSlicktopField;
                }
                set
                {
                    this.vehicleIsSlicktopField = value;
                }
            }

            /// <remarks/>
            public string ArrowboardType
            {
                get
                {
                    return this.arrowboardTypeField;
                }
                set
                {
                    this.arrowboardTypeField = value;
                }
            }

            /// <remarks/>
            public bool UseSteadyBurnLights
            {
                get
                {
                    return this.useSteadyBurnLightsField;
                }
                set
                {
                    this.useSteadyBurnLightsField = value;
                }
            }

            /// <remarks/>
            public byte DfltSirenLtsActivateAtLstg
            {
                get
                {
                    return this.dfltSirenLtsActivateAtLstgField;
                }
                set
                {
                    this.dfltSirenLtsActivateAtLstgField = value;
                }
            }

            /// <remarks/>
            public vcfrootMISCTakedowns Takedowns
            {
                get
                {
                    return this.takedownsField;
                }
                set
                {
                    this.takedownsField = value;
                }
            }

            /// <remarks/>
            public vcfrootMISCSceneLights SceneLights
            {
                get
                {
                    return this.sceneLightsField;
                }
                set
                {
                    this.sceneLightsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootMISCTakedowns
        {

            private bool allowUseField;

            private bool mirroredField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowUse
            {
                get
                {
                    return this.allowUseField;
                }
                set
                {
                    this.allowUseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Mirrored
            {
                get
                {
                    return this.mirroredField;
                }
                set
                {
                    this.mirroredField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootMISCSceneLights
        {

            private bool allowUseField;

            private bool illuminateSidesOnlyField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowUse
            {
                get
                {
                    return this.allowUseField;
                }
                set
                {
                    this.allowUseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool IlluminateSidesOnly
            {
                get
                {
                    return this.illuminateSidesOnlyField;
                }
                set
                {
                    this.illuminateSidesOnlyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootCRUISE
        {

            private bool disableAtLstg3Field;

            private vcfrootCRUISEUseExtras useExtrasField;

            /// <remarks/>
            public bool DisableAtLstg3
            {
                get
                {
                    return this.disableAtLstg3Field;
                }
                set
                {
                    this.disableAtLstg3Field = value;
                }
            }

            /// <remarks/>
            public vcfrootCRUISEUseExtras UseExtras
            {
                get
                {
                    return this.useExtrasField;
                }
                set
                {
                    this.useExtrasField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootCRUISEUseExtras
        {

            private bool extra1Field;

            private bool extra2Field;

            private bool extra3Field;

            private bool extra4Field;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Extra1
            {
                get
                {
                    return this.extra1Field;
                }
                set
                {
                    this.extra1Field = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Extra2
            {
                get
                {
                    return this.extra2Field;
                }
                set
                {
                    this.extra2Field = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Extra3
            {
                get
                {
                    return this.extra3Field;
                }
                set
                {
                    this.extra3Field = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Extra4
            {
                get
                {
                    return this.extra4Field;
                }
                set
                {
                    this.extra4Field = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootACORONAS
        {

            private vcfrootACORONASHeadlights headlightsField;

            private vcfrootACORONASTailLights tailLightsField;

            private vcfrootACORONASIndicatorsF indicatorsFField;

            private vcfrootACORONASIndicatorsB indicatorsBField;

            private vcfrootACORONASReverseLights reverseLightsField;

            /// <remarks/>
            public vcfrootACORONASHeadlights Headlights
            {
                get
                {
                    return this.headlightsField;
                }
                set
                {
                    this.headlightsField = value;
                }
            }

            /// <remarks/>
            public vcfrootACORONASTailLights TailLights
            {
                get
                {
                    return this.tailLightsField;
                }
                set
                {
                    this.tailLightsField = value;
                }
            }

            /// <remarks/>
            public vcfrootACORONASIndicatorsF IndicatorsF
            {
                get
                {
                    return this.indicatorsFField;
                }
                set
                {
                    this.indicatorsFField = value;
                }
            }

            /// <remarks/>
            public vcfrootACORONASIndicatorsB IndicatorsB
            {
                get
                {
                    return this.indicatorsBField;
                }
                set
                {
                    this.indicatorsBField = value;
                }
            }

            /// <remarks/>
            public vcfrootACORONASReverseLights ReverseLights
            {
                get
                {
                    return this.reverseLightsField;
                }
                set
                {
                    this.reverseLightsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootACORONASHeadlights
        {

            private byte dfltPatternField;

            private string colorLField;

            private string colorRField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte DfltPattern
            {
                get
                {
                    return this.dfltPatternField;
                }
                set
                {
                    this.dfltPatternField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ColorL
            {
                get
                {
                    return this.colorLField;
                }
                set
                {
                    this.colorLField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ColorR
            {
                get
                {
                    return this.colorRField;
                }
                set
                {
                    this.colorRField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootACORONASTailLights
        {

            private byte dfltPatternField;

            private string colorLField;

            private string colorRField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte DfltPattern
            {
                get
                {
                    return this.dfltPatternField;
                }
                set
                {
                    this.dfltPatternField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ColorL
            {
                get
                {
                    return this.colorLField;
                }
                set
                {
                    this.colorLField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ColorR
            {
                get
                {
                    return this.colorRField;
                }
                set
                {
                    this.colorRField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootACORONASIndicatorsF
        {

            private byte dfltPatternField;

            private string colorLField;

            private string colorRField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte DfltPattern
            {
                get
                {
                    return this.dfltPatternField;
                }
                set
                {
                    this.dfltPatternField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ColorL
            {
                get
                {
                    return this.colorLField;
                }
                set
                {
                    this.colorLField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ColorR
            {
                get
                {
                    return this.colorRField;
                }
                set
                {
                    this.colorRField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootACORONASIndicatorsB
        {

            private byte dfltPatternField;

            private string colorLField;

            private string colorRField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte DfltPattern
            {
                get
                {
                    return this.dfltPatternField;
                }
                set
                {
                    this.dfltPatternField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ColorL
            {
                get
                {
                    return this.colorLField;
                }
                set
                {
                    this.colorLField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ColorR
            {
                get
                {
                    return this.colorRField;
                }
                set
                {
                    this.colorRField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootACORONASReverseLights
        {

            private byte dfltPatternField;

            private string colorLField;

            private string colorRField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte DfltPattern
            {
                get
                {
                    return this.dfltPatternField;
                }
                set
                {
                    this.dfltPatternField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ColorL
            {
                get
                {
                    return this.colorLField;
                }
                set
                {
                    this.colorLField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ColorR
            {
                get
                {
                    return this.colorRField;
                }
                set
                {
                    this.colorRField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSOUNDS
        {

            private vcfrootSOUNDSMainHorn mainHornField;

            private vcfrootSOUNDSManTone1 manTone1Field;

            private vcfrootSOUNDSManTone2 manTone2Field;

            private vcfrootSOUNDSSrnTone1 srnTone1Field;

            private vcfrootSOUNDSSrnTone2 srnTone2Field;

            private vcfrootSOUNDSSrnTone3 srnTone3Field;

            private vcfrootSOUNDSSrnTone4 srnTone4Field;

            private vcfrootSOUNDSAuxSiren auxSirenField;

            private vcfrootSOUNDSPanicMde panicMdeField;

            /// <remarks/>
            public vcfrootSOUNDSMainHorn MainHorn
            {
                get
                {
                    return this.mainHornField;
                }
                set
                {
                    this.mainHornField = value;
                }
            }

            /// <remarks/>
            public vcfrootSOUNDSManTone1 ManTone1
            {
                get
                {
                    return this.manTone1Field;
                }
                set
                {
                    this.manTone1Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSOUNDSManTone2 ManTone2
            {
                get
                {
                    return this.manTone2Field;
                }
                set
                {
                    this.manTone2Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSOUNDSSrnTone1 SrnTone1
            {
                get
                {
                    return this.srnTone1Field;
                }
                set
                {
                    this.srnTone1Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSOUNDSSrnTone2 SrnTone2
            {
                get
                {
                    return this.srnTone2Field;
                }
                set
                {
                    this.srnTone2Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSOUNDSSrnTone3 SrnTone3
            {
                get
                {
                    return this.srnTone3Field;
                }
                set
                {
                    this.srnTone3Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSOUNDSSrnTone4 SrnTone4
            {
                get
                {
                    return this.srnTone4Field;
                }
                set
                {
                    this.srnTone4Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSOUNDSAuxSiren AuxSiren
            {
                get
                {
                    return this.auxSirenField;
                }
                set
                {
                    this.auxSirenField = value;
                }
            }

            /// <remarks/>
            public vcfrootSOUNDSPanicMde PanicMde
            {
                get
                {
                    return this.panicMdeField;
                }
                set
                {
                    this.panicMdeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSOUNDSMainHorn
        {

            private bool interruptsSirenField;

            private string audioStringField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool InterruptsSiren
            {
                get
                {
                    return this.interruptsSirenField;
                }
                set
                {
                    this.interruptsSirenField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AudioString
            {
                get
                {
                    return this.audioStringField;
                }
                set
                {
                    this.audioStringField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSOUNDSManTone1
        {

            private bool allowUseField;

            private string audioStringField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowUse
            {
                get
                {
                    return this.allowUseField;
                }
                set
                {
                    this.allowUseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AudioString
            {
                get
                {
                    return this.audioStringField;
                }
                set
                {
                    this.audioStringField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSOUNDSManTone2
        {

            private bool allowUseField;

            private string audioStringField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowUse
            {
                get
                {
                    return this.allowUseField;
                }
                set
                {
                    this.allowUseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AudioString
            {
                get
                {
                    return this.audioStringField;
                }
                set
                {
                    this.audioStringField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSOUNDSSrnTone1
        {

            private bool allowUseField;

            private string audioStringField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowUse
            {
                get
                {
                    return this.allowUseField;
                }
                set
                {
                    this.allowUseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AudioString
            {
                get
                {
                    return this.audioStringField;
                }
                set
                {
                    this.audioStringField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSOUNDSSrnTone2
        {

            private bool allowUseField;

            private string audioStringField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowUse
            {
                get
                {
                    return this.allowUseField;
                }
                set
                {
                    this.allowUseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AudioString
            {
                get
                {
                    return this.audioStringField;
                }
                set
                {
                    this.audioStringField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSOUNDSSrnTone3
        {

            private bool allowUseField;

            private string audioStringField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowUse
            {
                get
                {
                    return this.allowUseField;
                }
                set
                {
                    this.allowUseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AudioString
            {
                get
                {
                    return this.audioStringField;
                }
                set
                {
                    this.audioStringField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSOUNDSSrnTone4
        {

            private bool allowUseField;

            private string audioStringField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowUse
            {
                get
                {
                    return this.allowUseField;
                }
                set
                {
                    this.allowUseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AudioString
            {
                get
                {
                    return this.audioStringField;
                }
                set
                {
                    this.audioStringField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSOUNDSAuxSiren
        {

            private bool allowUseField;

            private string audioStringField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowUse
            {
                get
                {
                    return this.allowUseField;
                }
                set
                {
                    this.allowUseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AudioString
            {
                get
                {
                    return this.audioStringField;
                }
                set
                {
                    this.audioStringField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSOUNDSPanicMde
        {

            private bool allowUseField;

            private string audioStringField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool AllowUse
            {
                get
                {
                    return this.allowUseField;
                }
                set
                {
                    this.allowUseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AudioString
            {
                get
                {
                    return this.audioStringField;
                }
                set
                {
                    this.audioStringField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNL
        {

            private vcfrootWRNLPresetPatterns presetPatternsField;

            private vcfrootWRNLForcedPatterns forcedPatternsField;

            private vcfrootWRNLScanPatternCustomPool scanPatternCustomPoolField;

            private string lightingFormatField;

            /// <remarks/>
            public vcfrootWRNLPresetPatterns PresetPatterns
            {
                get
                {
                    return this.presetPatternsField;
                }
                set
                {
                    this.presetPatternsField = value;
                }
            }

            /// <remarks/>
            public vcfrootWRNLForcedPatterns ForcedPatterns
            {
                get
                {
                    return this.forcedPatternsField;
                }
                set
                {
                    this.forcedPatternsField = value;
                }
            }

            /// <remarks/>
            public vcfrootWRNLScanPatternCustomPool ScanPatternCustomPool
            {
                get
                {
                    return this.scanPatternCustomPoolField;
                }
                set
                {
                    this.scanPatternCustomPoolField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string LightingFormat
            {
                get
                {
                    return this.lightingFormatField;
                }
                set
                {
                    this.lightingFormatField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLPresetPatterns
        {

            private vcfrootWRNLPresetPatternsLstg3 lstg3Field;

            /// <remarks/>
            public vcfrootWRNLPresetPatternsLstg3 Lstg3
            {
                get
                {
                    return this.lstg3Field;
                }
                set
                {
                    this.lstg3Field = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLPresetPatternsLstg3
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLForcedPatterns
        {

            private vcfrootWRNLForcedPatternsMainHorn mainHornField;

            private vcfrootWRNLForcedPatternsSrnTone1 srnTone1Field;

            private vcfrootWRNLForcedPatternsSrnTone2 srnTone2Field;

            private vcfrootWRNLForcedPatternsSrnTone3 srnTone3Field;

            private vcfrootWRNLForcedPatternsSrnTone4 srnTone4Field;

            private vcfrootWRNLForcedPatternsPanicMde panicMdeField;

            private vcfrootWRNLForcedPatternsOutOfVeh outOfVehField;

            /// <remarks/>
            public vcfrootWRNLForcedPatternsMainHorn MainHorn
            {
                get
                {
                    return this.mainHornField;
                }
                set
                {
                    this.mainHornField = value;
                }
            }

            /// <remarks/>
            public vcfrootWRNLForcedPatternsSrnTone1 SrnTone1
            {
                get
                {
                    return this.srnTone1Field;
                }
                set
                {
                    this.srnTone1Field = value;
                }
            }

            /// <remarks/>
            public vcfrootWRNLForcedPatternsSrnTone2 SrnTone2
            {
                get
                {
                    return this.srnTone2Field;
                }
                set
                {
                    this.srnTone2Field = value;
                }
            }

            /// <remarks/>
            public vcfrootWRNLForcedPatternsSrnTone3 SrnTone3
            {
                get
                {
                    return this.srnTone3Field;
                }
                set
                {
                    this.srnTone3Field = value;
                }
            }

            /// <remarks/>
            public vcfrootWRNLForcedPatternsSrnTone4 SrnTone4
            {
                get
                {
                    return this.srnTone4Field;
                }
                set
                {
                    this.srnTone4Field = value;
                }
            }

            /// <remarks/>
            public vcfrootWRNLForcedPatternsPanicMde PanicMde
            {
                get
                {
                    return this.panicMdeField;
                }
                set
                {
                    this.panicMdeField = value;
                }
            }

            /// <remarks/>
            public vcfrootWRNLForcedPatternsOutOfVeh OutOfVeh
            {
                get
                {
                    return this.outOfVehField;
                }
                set
                {
                    this.outOfVehField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLForcedPatternsMainHorn
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLForcedPatternsSrnTone1
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLForcedPatternsSrnTone2
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLForcedPatternsSrnTone3
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLForcedPatternsSrnTone4
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLForcedPatternsPanicMde
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLForcedPatternsOutOfVeh
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootWRNLScanPatternCustomPool
        {

            private byte[] patternField;

            private bool enabledField;

            private bool sequentialField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Pattern")]
            public byte[] Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Sequential
            {
                get
                {
                    return this.sequentialField;
                }
                set
                {
                    this.sequentialField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRML
        {

            private vcfrootPRMLPresetPatterns presetPatternsField;

            private vcfrootPRMLForcedPatterns forcedPatternsField;

            private vcfrootPRMLScanPatternCustomPool scanPatternCustomPoolField;

            private string lightingFormatField;

            private string extrasActiveAtLstg2Field;

            /// <remarks/>
            public vcfrootPRMLPresetPatterns PresetPatterns
            {
                get
                {
                    return this.presetPatternsField;
                }
                set
                {
                    this.presetPatternsField = value;
                }
            }

            /// <remarks/>
            public vcfrootPRMLForcedPatterns ForcedPatterns
            {
                get
                {
                    return this.forcedPatternsField;
                }
                set
                {
                    this.forcedPatternsField = value;
                }
            }

            /// <remarks/>
            public vcfrootPRMLScanPatternCustomPool ScanPatternCustomPool
            {
                get
                {
                    return this.scanPatternCustomPoolField;
                }
                set
                {
                    this.scanPatternCustomPoolField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string LightingFormat
            {
                get
                {
                    return this.lightingFormatField;
                }
                set
                {
                    this.lightingFormatField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ExtrasActiveAtLstg2
            {
                get
                {
                    return this.extrasActiveAtLstg2Field;
                }
                set
                {
                    this.extrasActiveAtLstg2Field = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLPresetPatterns
        {

            private vcfrootPRMLPresetPatternsLstg2 lstg2Field;

            private vcfrootPRMLPresetPatternsLstg3 lstg3Field;

            /// <remarks/>
            public vcfrootPRMLPresetPatternsLstg2 Lstg2
            {
                get
                {
                    return this.lstg2Field;
                }
                set
                {
                    this.lstg2Field = value;
                }
            }

            /// <remarks/>
            public vcfrootPRMLPresetPatternsLstg3 Lstg3
            {
                get
                {
                    return this.lstg3Field;
                }
                set
                {
                    this.lstg3Field = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLPresetPatternsLstg2
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLPresetPatternsLstg3
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLForcedPatterns
        {

            private vcfrootPRMLForcedPatternsMainHorn mainHornField;

            private vcfrootPRMLForcedPatternsSrnTone1 srnTone1Field;

            private vcfrootPRMLForcedPatternsSrnTone2 srnTone2Field;

            private vcfrootPRMLForcedPatternsSrnTone3 srnTone3Field;

            private vcfrootPRMLForcedPatternsSrnTone4 srnTone4Field;

            private vcfrootPRMLForcedPatternsPanicMde panicMdeField;

            private vcfrootPRMLForcedPatternsOutOfVeh outOfVehField;

            /// <remarks/>
            public vcfrootPRMLForcedPatternsMainHorn MainHorn
            {
                get
                {
                    return this.mainHornField;
                }
                set
                {
                    this.mainHornField = value;
                }
            }

            /// <remarks/>
            public vcfrootPRMLForcedPatternsSrnTone1 SrnTone1
            {
                get
                {
                    return this.srnTone1Field;
                }
                set
                {
                    this.srnTone1Field = value;
                }
            }

            /// <remarks/>
            public vcfrootPRMLForcedPatternsSrnTone2 SrnTone2
            {
                get
                {
                    return this.srnTone2Field;
                }
                set
                {
                    this.srnTone2Field = value;
                }
            }

            /// <remarks/>
            public vcfrootPRMLForcedPatternsSrnTone3 SrnTone3
            {
                get
                {
                    return this.srnTone3Field;
                }
                set
                {
                    this.srnTone3Field = value;
                }
            }

            /// <remarks/>
            public vcfrootPRMLForcedPatternsSrnTone4 SrnTone4
            {
                get
                {
                    return this.srnTone4Field;
                }
                set
                {
                    this.srnTone4Field = value;
                }
            }

            /// <remarks/>
            public vcfrootPRMLForcedPatternsPanicMde PanicMde
            {
                get
                {
                    return this.panicMdeField;
                }
                set
                {
                    this.panicMdeField = value;
                }
            }

            /// <remarks/>
            public vcfrootPRMLForcedPatternsOutOfVeh OutOfVeh
            {
                get
                {
                    return this.outOfVehField;
                }
                set
                {
                    this.outOfVehField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLForcedPatternsMainHorn
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLForcedPatternsSrnTone1
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLForcedPatternsSrnTone2
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLForcedPatternsSrnTone3
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLForcedPatternsSrnTone4
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLForcedPatternsPanicMde
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLForcedPatternsOutOfVeh
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootPRMLScanPatternCustomPool
        {

            private byte[] patternField;

            private bool enabledField;

            private bool sequentialField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Pattern")]
            public byte[] Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Sequential
            {
                get
                {
                    return this.sequentialField;
                }
                set
                {
                    this.sequentialField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECL
        {

            private vcfrootSECLPresetPatterns presetPatternsField;

            private vcfrootSECLForcedPatterns forcedPatternsField;

            private vcfrootSECLScanPatternCustomPool scanPatternCustomPoolField;

            private string lightingFormatField;

            private bool disableAtLstg3Field;

            /// <remarks/>
            public vcfrootSECLPresetPatterns PresetPatterns
            {
                get
                {
                    return this.presetPatternsField;
                }
                set
                {
                    this.presetPatternsField = value;
                }
            }

            /// <remarks/>
            public vcfrootSECLForcedPatterns ForcedPatterns
            {
                get
                {
                    return this.forcedPatternsField;
                }
                set
                {
                    this.forcedPatternsField = value;
                }
            }

            /// <remarks/>
            public vcfrootSECLScanPatternCustomPool ScanPatternCustomPool
            {
                get
                {
                    return this.scanPatternCustomPoolField;
                }
                set
                {
                    this.scanPatternCustomPoolField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string LightingFormat
            {
                get
                {
                    return this.lightingFormatField;
                }
                set
                {
                    this.lightingFormatField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool DisableAtLstg3
            {
                get
                {
                    return this.disableAtLstg3Field;
                }
                set
                {
                    this.disableAtLstg3Field = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLPresetPatterns
        {

            private vcfrootSECLPresetPatternsLstg1 lstg1Field;

            private vcfrootSECLPresetPatternsLstg2 lstg2Field;

            private vcfrootSECLPresetPatternsLstg3 lstg3Field;

            /// <remarks/>
            public vcfrootSECLPresetPatternsLstg1 Lstg1
            {
                get
                {
                    return this.lstg1Field;
                }
                set
                {
                    this.lstg1Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSECLPresetPatternsLstg2 Lstg2
            {
                get
                {
                    return this.lstg2Field;
                }
                set
                {
                    this.lstg2Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSECLPresetPatternsLstg3 Lstg3
            {
                get
                {
                    return this.lstg3Field;
                }
                set
                {
                    this.lstg3Field = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLPresetPatternsLstg1
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLPresetPatternsLstg2
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLPresetPatternsLstg3
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLForcedPatterns
        {

            private vcfrootSECLForcedPatternsMainHorn mainHornField;

            private vcfrootSECLForcedPatternsSrnTone1 srnTone1Field;

            private vcfrootSECLForcedPatternsSrnTone2 srnTone2Field;

            private vcfrootSECLForcedPatternsSrnTone3 srnTone3Field;

            private vcfrootSECLForcedPatternsSrnTone4 srnTone4Field;

            private vcfrootSECLForcedPatternsPanicMde panicMdeField;

            private vcfrootSECLForcedPatternsOutOfVeh outOfVehField;

            /// <remarks/>
            public vcfrootSECLForcedPatternsMainHorn MainHorn
            {
                get
                {
                    return this.mainHornField;
                }
                set
                {
                    this.mainHornField = value;
                }
            }

            /// <remarks/>
            public vcfrootSECLForcedPatternsSrnTone1 SrnTone1
            {
                get
                {
                    return this.srnTone1Field;
                }
                set
                {
                    this.srnTone1Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSECLForcedPatternsSrnTone2 SrnTone2
            {
                get
                {
                    return this.srnTone2Field;
                }
                set
                {
                    this.srnTone2Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSECLForcedPatternsSrnTone3 SrnTone3
            {
                get
                {
                    return this.srnTone3Field;
                }
                set
                {
                    this.srnTone3Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSECLForcedPatternsSrnTone4 SrnTone4
            {
                get
                {
                    return this.srnTone4Field;
                }
                set
                {
                    this.srnTone4Field = value;
                }
            }

            /// <remarks/>
            public vcfrootSECLForcedPatternsPanicMde PanicMde
            {
                get
                {
                    return this.panicMdeField;
                }
                set
                {
                    this.panicMdeField = value;
                }
            }

            /// <remarks/>
            public vcfrootSECLForcedPatternsOutOfVeh OutOfVeh
            {
                get
                {
                    return this.outOfVehField;
                }
                set
                {
                    this.outOfVehField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLForcedPatternsMainHorn
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLForcedPatternsSrnTone1
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLForcedPatternsSrnTone2
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLForcedPatternsSrnTone3
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLForcedPatternsSrnTone4
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLForcedPatternsPanicMde
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLForcedPatternsOutOfVeh
        {

            private bool enabledField;

            private byte patternField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class vcfrootSECLScanPatternCustomPool
        {

            private byte[] patternField;

            private bool enabledField;

            private bool sequentialField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Pattern")]
            public byte[] Pattern
            {
                get
                {
                    return this.patternField;
                }
                set
                {
                    this.patternField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Enabled
            {
                get
                {
                    return this.enabledField;
                }
                set
                {
                    this.enabledField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool Sequential
            {
                get
                {
                    return this.sequentialField;
                }
                set
                {
                    this.sequentialField = value;
                }
            }
        }


    }
}