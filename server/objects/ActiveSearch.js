class ActiveSearch{
    ID;
    Resolved;
    Participants = [];
    EateryOptions = [];

    constructor(startingParticipant, eateryOptions) {
        this.ID = 'CODE_' + Math.random().toString(36).substr(2, 9);
        this.ID = Math.random().toString(36).substr(2, 2);//test

        this.addParticipant(startingParticipant);
        this.EateryOptions = eateryOptions;


    }

    addParticipant(user){
        this.Participants.push(user);
    }

    castVote(username, EateryOptionID){
        //go through eateryOptions, where EateryOptionsID matches selected eateryOption.ID add username to eateryOption.Votes
        for (let i = 0; i < this.EateryOptions.length; i++)
        {
            let selectedEateryOption = this.EateryOptions[i];

            if(selectedEateryOption.ID === EateryOptionID){
                selectedEateryOption.recordVote(username);
            }
        }
    }

    getVotes(){
        //for each eatery option, get number of votes, spit it out to the console
        console.log("[VOTE] Printing votes...");
        for (let i = 0; i < this.EateryOptions.length; i++)
        {
            let selectedEateryOption = this.EateryOptions[i];
            console.log("Title: " + selectedEateryOption.Title + ", Votes: " + this.getNumberOfUniqueVotes(selectedEateryOption.Votes));
        }
    }

    checkForMatch(){
        //go through eateryOptions, where an eateryOption has a vote from each participant and the total number of votes is greater than one, return the eatery option
        for (let i = 0; i < this.EateryOptions.length; i++)
        {
            let selectedEateryOption = this.EateryOptions[i];
            let numberOfUniqueVotes = this.getNumberOfUniqueVotes(selectedEateryOption.Votes);

            if(numberOfUniqueVotes > 1 && numberOfUniqueVotes === this.Participants.length){//if every participant has voted for a restaurant, and there is more than one participant in the search
                return selectedEateryOption;
            }
        }

        return null;
    }

    getNumberOfUniqueVotes(votes){
        let voterPool = [];
        for(let i = 0; i < votes.length; i++){
            let name = votes[i];

            if(!voterPool.find(function (username) {//if name is not already in voterPool
                return (username === name);
            })){
                voterPool.push(name);// add it to the voterPool
            }

            // if(!voterPool.find(name)){//if name is not already in voterPool
            //     voterPool.push(name);// add it to the voterPool
            // }
        }
        return voterPool.length;
    }

}
module.exports = ActiveSearch;