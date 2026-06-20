import type { HubNotification } from "../models/HubNotification";

type HangingNotification = { handler: Function; notification: HubNotification<any> };

export let hangingNotifications: HangingNotification[] = [];

export function addHangingNotification(notification: HangingNotification) {
    hangingNotifications.push(notification);
}

export function clearHangingNotifications() {
    hangingNotifications = [];
}