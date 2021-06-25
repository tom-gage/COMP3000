class User{
    Username = "";
    Password = "";
    current_WSID = "";

    constructor(Username, Password, Current_WSID) {
        this.Username = Username;
        this.Password = Password;
        this.current_WSID = Current_WSID;
    }

    testSendWSMessage(ws, message){
        console.log("testing...");
        // let message = new Message(1, "debugMessage", "if you're reading this, the test has succeeded", []);
        ws.send(JSON.stringify(message));
        console.log("test complete!");
    }


}
module.exports = User;