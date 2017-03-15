// Copyright (c) 2013-2016 Cemalettin Dervis, MIT License.
// https://github.com/cemdervis/SharpConfig

using System;
using System.Collections.Generic;

namespace SharpConfig
{
    /// <summary>
    /// Represents the base class of all elements
    /// that exist in a <see cref="Configuration"/>,
    /// such as sections and settings.
    /// </summary>
    public abstract class ConfigurationElement
    {
        internal ConfigurationElement(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            Name = name;
        }

        /// <summary>
        /// Gets the name of this element.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the comment of this element.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets the comment above this element.
        /// </summary>
        public string PreComment { get; set; }

        /// <summary>
        /// Gets the string representation of the element without its comments.
        /// </summary>
        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// Gets the string representation of the element.
        /// </summary>
        ///
        /// <param name="includeComments">Specify true to include the comments in the string; false otherwise.</param>
        public string ToString(bool includeComments)
        {
            string stringExpr = GetStringExpression();

            if (includeComments)
            {
                if (Comment != null && PreComment != null)
                {
                    // Include inline comment and pre-comments.
                    return string.Format("{0}{1}{2} {3}",
                        GetFormattedPreComment(),   // {0}
                        Environment.NewLine,        // {1}
                        stringExpr,                 // {2}
                        GetFormattedComment()       // {3}
                        );
                }
                else if (Comment != null)
                {
                    // Include only the inline comment.
                    return string.Format("{0} {1}",
                        stringExpr,                 // {0}
                        GetFormattedComment()       // {1}
                        );
                }
                else if (PreComment != null)
                {
                    // Include only the pre-comments.
                    return string.Format("{0}{1}{2}",
                        GetFormattedPreComment(),   // {0}
                        Environment.NewLine,        // {1}
                        stringExpr                  // {2}
                        );
                }
            }

            // In every other case, just return the expression.
            return stringExpr;
        }

        // Gets a formatted comment string that is ready to be written to a config file.
        private string GetFormattedComment()
        {
            // Only get the first line of the inline comment.
            string comment = Comment;

            int iNewLine = Comment.IndexOfAny(Environment.NewLine.ToCharArray());
            if (iNewLine >= 0)
                comment = comment.Substring(0, iNewLine);

            return (Configuration.PreferredCommentChar + " " + comment);
        }

        // Gets a formatted pre-comment string that is ready
        // to be written to a config file.
        private string GetFormattedPreComment()
        {
            string[] lines = PreComment.Split(
                new[] { "\r\n", "\n" },
                StringSplitOptions.None
                );
            
            return string.Join(
                Environment.NewLine,
                Array.ConvertAll(lines, s => Configuration.PreferredCommentChar + " " + s)
                );
        }

        /// <summary>
        /// Gets the element's expression as a string.
        /// An example for a section would be "[Section]".
        /// </summary>
        /// <returns>The element's expression as a string.</returns>
        protected abstract string GetStringExpression();
    }
}
