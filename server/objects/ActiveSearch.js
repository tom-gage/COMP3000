class ActiveSearch{
    //the active search class contains the participants, eatery options, and votes that make up a search

    ID;
    Resolved;
    Participants = [];
    EateryOptions = [];

    constructor(startingParticipant, eateryOptions) {
        //initialises the searches ID
        this.ID = Math.random().toString(36).substr(2, 4);//test

        //adds the participant that created the search to the participants list
        this.addParticipant(startingParticipant);

        this.EateryOptions = eateryOptions;
    }

    //adds a user to the participants array
    addParticipant(user){
        this.Participants.push(user);
    }

    //records a vote for an eatery
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

    //prints the current votes in the console, for debugging
    showVotes(){
        //for each eatery option, get number of votes, spit it out to the console
        console.log("[VOTE] Printing votes...");
        for (let i = 0; i < this.EateryOptions.length; i++)
        {
            let selectedEateryOption = this.EateryOptions[i];
            console.log("Title: " + selectedEateryOption.Title + ", Votes: " + this.getNumberOfUniqueVotes(selectedEateryOption.Votes));
        }
    }

    //checks to see if any of the participants have voted for the same eatery
    checkForMatch(){
        //go through eateryOptions, where an eateryOption has a vote from each participant and the total number of votes is greater than one, return the eatery option
        console.log("checking for a match...")
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

    //gets the number of unique votes for an eatery
    getNumberOfUniqueVotes(votes){
        let voterPool = [];
        for(let i = 0; i < votes.length; i++){
            let name = votes[i];

            if(!voterPool.find(function (username) {//if name is not already in voterPool
                return (username === name);
            })){
                voterPool.push(name);// add it to the voterPool
            }

        }
        return voterPool.length;
    }

}
module.exports = ActiveSearch;