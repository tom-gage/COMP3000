class ActiveSearch{
    ID;
    Resolved;
    Participants = [];
    EateryOptions = [];
    voteMap;

    constructor(startingParticipant, eateryOptions) {
        this.ID = 'CODE_' + Math.random().toString(36).substr(2, 9);

        this.addParticipant(startingParticipant);
        this.EateryOptions = eateryOptions;
        this.voteMap = new Map();

        this.initialiseVoteMap();
    }

    addParticipant(user){
        this.Participants.push(user);
    }

    initialiseVoteMap(){
        for(let i = 0; i > this.EateryOptions.length; i++){
            this.voteMap.put(this.EateryOptions[i], 0);
        }
        console.log(this.voteMap);
    }
}
module.exports = ActiveSearch;