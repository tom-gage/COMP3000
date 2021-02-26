class Message{
    ID;
    type;
    Body;

    constructor(ID, type, Body) {
        this.ID = ID;
        this.type = type;
        this.Body = Body;
    }
}
module.exports = Message;