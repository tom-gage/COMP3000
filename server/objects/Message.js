class Message{
    ID;
    Type;
    Body;

    constructor(ID, Type, Body) {
        this.ID = ID;
        this.Type = Type;
        this.Body = Body;
    }
}
module.exports = Message;