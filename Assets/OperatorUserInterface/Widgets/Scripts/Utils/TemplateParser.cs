using System.Collections.Generic;
using UnityEngine;

namespace Widgets
{
    public static class TemplateParser
    {
        /// <summary>
        /// Parses all widget template json files in resources folder and deserializes them into RosJsonMessages as contexts for initialization by the factory.
        /// </summary>
        /// <returns></returns>
        public static List<RosJsonMessage> ParseAllWidgetTemplates()
        {
            List<RosJsonMessage> widgetContexts = new List<RosJsonMessage>();

            TextAsset[] widgetTemplates = Resources.LoadAll<TextAsset>("JsonTemplates");
            foreach (TextAsset widgetTemplate in widgetTemplates)
            {
                RosJsonMessage parsedWidgetContext = ParseWidgetTemplate(widgetTemplate);

                if (parsedWidgetContext == null)
                {
                    continue;
                }

                widgetContexts.Add(ParseWidgetTemplate(widgetTemplate));
            }

            return widgetContexts;
        }

        /// <summary>
        /// Parse a single widget template and create the RosJsonMessage object.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private static RosJsonMessage ParseWidgetTemplate(TextAsset asset)
        {
            RosJsonMessage parsedContext = JsonUtility.FromJson<RosJsonMessage>(asset.text);
            if (parsedContext == null)
            {
                Debug.LogWarning("Json " + asset.text + " is faulty");
                return null;
            }

            return parsedContext;
        }
    }
}
