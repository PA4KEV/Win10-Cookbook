using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class TileUpdateTask : IBackgroundTask
    {
        
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            //BackgroundTaskDeferral _deferral = taskInstance.GetDeferral(); // used to safely use async methods
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;

            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            XmlNodeList textElements = toastXml.GetElementsByTagName("text");
            textElements[0].AppendChild(toastXml.CreateTextNode("My first Task - Yeah"));
            textElements[1].AppendChild(toastXml.CreateTextNode("I'm a message from your background task!"));
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toastXml));

            //_deferral.Complete();
        }

        private void UpdateTile(string infoString)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();
            Windows.Data.Xml.Dom.XmlDocument xml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text04);

            xml.GetElementsByTagName("text")[0].InnerText = infoString;
            updater.Update(new TileNotification(xml));
        }
    }
}
