using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets
{
    public class Factory : Singleton<Factory>
    {
        public GameObject toastrDesignPrefab;
        public GameObject textDesignPrefab;
        public GameObject iconDesignPrefab;
        public GameObject graphDesignPrefab;

        private GameObject widgetParentGameObject;

        Dictionary<string, Texture2D> icons;

        /// <summary>
        /// Receives all widget templates from parser and creates corresponding widgets objects.
        /// </summary>
        /// <returns>List of created widget objects</returns>
        public List<Widget> CreateWidgetsAtStartup()
        {
            icons = FetchAllIcons();

            CreateWidgetParentGameObject();

            List<RosJsonMessage> widgetContexts = TemplateParser.ParseAllWidgetTemplates();

            List<Widget> widgets = new List<Widget>();

            foreach (RosJsonMessage widgetContext in widgetContexts)
            {
                Widget createdWidget = CreateWidgetFromContext(widgetContext, widgets);

                if (createdWidget == null)
                {
                    Debug.LogWarning("widget is null");
                    continue;
                }

                widgets.Add(createdWidget);
            }

            return widgets;
        }
        
        /// <summary>
        /// Creates an empty gameobject for every widget for better hierarchical structure in scene graph
        /// </summary>
        private void CreateWidgetParentGameObject()
        {
            widgetParentGameObject = new GameObject("Widgets");
        }

        /// <summary>
        /// Opens all registered icons from the resources folder.
        /// </summary>
        /// <returns>Returns a dictionary of the icons as Texture2D with their names as keys.</returns>
        private Dictionary<string, Texture2D> FetchAllIcons()
        {
            Texture2D[] iconsArray = Resources.LoadAll<Texture2D>("Icons");
            Dictionary<string, Texture2D> iconsDictionary = new Dictionary<string, Texture2D>();

            foreach (Texture2D icon in iconsArray)
            {
                iconsDictionary.Add(icon.name, icon);
            }

            return iconsDictionary;
        }

        /// <summary>
        /// Finds a subset of icons from all registered icons by their names as keys.
        /// </summary>
        /// <param name="iconNames">Names of icons to find in registered icons</param>
        /// <returns>Subset of icons matching the given names</returns>
        private Dictionary<string, Texture2D> FindIconsWithName(string[] iconNames)
        {
            Dictionary<string, Texture2D> iconsFound = new Dictionary<string, Texture2D>();

            foreach (string name in iconNames)
            {
                if (icons.ContainsKey(name))
                {
                    Texture2D iconFound;
                    icons.TryGetValue(name, out iconFound);
                    iconsFound.Add(name, iconFound);
                }
            }

            return iconsFound;
        }

        /// <summary>
        /// Checks, if widget id of given context is already taken
        /// </summary>
        /// <param name="newWidgetContext">Widget context of new widget to register</param>
        /// <param name="existingWidgets">List of existing widgets</param>
        /// <returns></returns>
        private bool IsWidgetIdUnique(RosJsonMessage newWidgetContext, List<Widget> existingWidgets)
        {
            foreach (Widget existingWidget in existingWidgets)
            {
                if (existingWidget.GetID() == newWidgetContext.id)
                {                    
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Create a new widget object from a given context.
        /// </summary>
        /// <param name="widgetContext">Context of new widget to create</param>
        /// <param name="existingWidgets">List of already existing widgets, to check for duplicate ids</param>
        /// <returns></returns>
        public Widget CreateWidgetFromContext(RosJsonMessage widgetContext, List<Widget> existingWidgets)
        {
            if (IsWidgetIdUnique(widgetContext, existingWidgets) == false)
            {
                Debug.LogWarning("duplicate ID: " + widgetContext.id + " in widget templates");
                return null;
            }

            GameObject widgetGameObject = new GameObject();
            widgetGameObject.name = widgetContext.title;
            widgetGameObject.transform.SetParent(widgetParentGameObject.transform, false);

            switch (widgetContext.type)
            {
                case "Graph":
                    GraphWidget graphWidget = widgetGameObject.AddComponent<GraphWidget>();
                    graphWidget.Init(widgetContext, graphDesignPrefab);
                    return graphWidget;

                case "Toastr":
                    ToastrWidget toastrWidget = widgetGameObject.AddComponent<ToastrWidget>();
                    toastrWidget.Init(widgetContext, toastrDesignPrefab);
                    return toastrWidget;

                case "Icon":
                    IconWidget iconWidget = widgetGameObject.AddComponent<IconWidget>();
                    Dictionary<string, Texture2D> iconsForThisWidget = FindIconsWithName(widgetContext.icons);
                    iconWidget.Init(widgetContext, iconDesignPrefab, iconsForThisWidget);           
                    return iconWidget;
                    
                case "Text":
                    TextWidget textWidget = widgetGameObject.AddComponent<TextWidget>();
                    textWidget.Init(widgetContext, textDesignPrefab);
                    return textWidget;

                default:
                    Debug.LogWarning("Type not defined: " + widgetContext.type);
                    Destroy(widgetGameObject);
                    return null;
            }
        }
    }
}