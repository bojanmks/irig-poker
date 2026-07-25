import type { HubNotification } from "../models/HubNotification";

type HangingNotification = { handler: (...args: unknown[]) => unknown; notification: HubNotification<unknown> };

export let hangingNotifications: HangingNotification[] = [];

export function addHangingNotification(notification: HangingNotification) {
    hangingNotifications.push(notification);
}

export function clearHangingNotifications() {
    hangingNotifications = [];
}