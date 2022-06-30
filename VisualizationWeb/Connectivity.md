# Connectivity

## WebSocket-Client (Responder) <--> RaspberryPI (Sender)

Connection String Form: *ws://{static IP-address Raspberry}:{defined port}*<br>
Connection String Example: *ws://192.168.1.1:8080*

The websocket client connects to the Raspberry.<br>
After connecting, the client receives data from the Raspberry, responding with an 
*ack* (Acknowledged) status response.

If the response is *error* instead, something went wrong. Check:
- Does the SocketClient class (Application) handle the received data as intended?
- Is the received data sent in a malformed/invalid form?


## WebSocket-Server (Sender) <--> Browser (Receiver)

Connection String Form: ??<br>
Connection String Example: *ws://localhost:8109/Connection* ??

The browser(client) connects to the websocket server.<br>
After connecting, the server sends data for visualization to the browser.
Said data is then visible on the Dashboard site on the clients system.