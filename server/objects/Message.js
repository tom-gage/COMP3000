class Message{
    //used to transfer information between the server and the application

    ID;
    type;
    Body;
    Items = [];

    constructor(ID, type, Body, Items) {
        this.ID = ID;
        this.type = type;
        this.Body = Body;
        this.Items = Items;
    }
}
module.exports = Message;