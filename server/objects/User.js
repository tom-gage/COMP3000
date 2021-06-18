class User{
    Username;
    Password;
    current_WSID;

    constructor(Username, Password, Current_WSID) {
        this.Username = Username;
        this.Password = Password;
        this.current_WSID = Current_WSID;
    }
}
module.exports = User;