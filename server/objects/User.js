class User{
    //represents a user

    Username = "";
    Password = "";
    current_WSID = "";

    constructor(Username, Password, Current_WSID) {
        this.Username = Username;
        this.Password = Password;
        this.current_WSID = Current_WSID;
    }

    testSendWSMessage(ws, message){
        ws.send(JSON.stringify(message));
    }


}
module.exports = User;