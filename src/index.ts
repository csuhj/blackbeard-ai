import * as signalR from "@microsoft/signalr";
import "./css/main.css";

const divMessages: HTMLDivElement = document.querySelector("#divMessages");
const tbMessage: HTMLInputElement = document.querySelector("#tbMessage");
const btnSend: HTMLButtonElement = document.querySelector("#btnSend");
const username = new Date().getTime().toString();

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .build();

connection.on("messageReceived", (username: string, messagePart: string, isNewMessage: boolean) => {
  if (isNewMessage) {
    const m = document.createElement("div");
    m.innerHTML = `<div class="message-author">${username}</div><div>${messagePart}</div>`;
    divMessages.appendChild(m);
  } else {
    const m = divMessages.children.item(divMessages.children.length - 1);
    const currentMessage = m.children.item(1);
    currentMessage.innerHTML = currentMessage.innerHTML + messagePart;
  }
  divMessages.scrollTop = divMessages.scrollHeight;
});

connection.start().catch((err) => document.write(err));

tbMessage.addEventListener("keyup", (e: KeyboardEvent) => {
  if (e.key === "Enter") {
    send();
  }
});

btnSend.addEventListener("click", send);

function send() {
  connection.send("newMessage", username, tbMessage.value)
    .then(() => (tbMessage.value = ""));
}