using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications;
using Unity.Notifications.Android;

public class NotificationsManager : MonoBehaviour
{
    public void Show()
    {
        // ä�� ���
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        // �˸� ����
        var notification = new AndroidNotification();

        notification.Title = "Test";
        notification.Text = "This is a test for android notification.";
        notification.FireTime = System.DateTime.Now.AddSeconds(10);

        notification.SmallIcon = "icon_0";
        notification.LargeIcon = "icon_1";

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
}
