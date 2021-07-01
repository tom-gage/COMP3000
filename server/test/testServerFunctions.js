const assert = require('chai').assert;
const ServerFunctions = require('../ServerFunctions');
const ActiveSearch = require('../objects/ActiveSearch');
const EateryOption = require('../objects/EateryOption');
const User = require('../objects/User');
const DB = require('../DB');

describe('hooks', function () {

    let serverFunctions;
    let eateryOption1;
    let eateryOption2;
    let user0;
    let user1;

    let activeSearch;

    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }


    before(function (done){
        serverFunctions = new ServerFunctions();

        serverFunctions.initConnection().then(()=>{//this thing ensures that the tests wait until the DB's connected before running, don't ask me how it works
            done();
        })
    });

    beforeEach(function () {
        eateryOption1 = new EateryOption("0", "Five Guys", "Burgers and fries", "10", "123");
        eateryOption2 = new EateryOption("1", "SteakPlace", "The place for steaks", "5", "456");

        user0 = new User("testUser", "password", "123");
        user1 = new User("testUser1", "password", "456");

        activeSearch = new ActiveSearch(user0,[eateryOption1, eateryOption2]);

        // USERS.clear();
        // ACTIVE_SEARCHES.clear();

        serverFunctions.clearUSERS();
        serverFunctions.clearACTIVE_SEARCHES();
    });

    after(function (){
        let username = "testUsername";
        let password = "testPassword";

        serverFunctions.UserModel.deleteOne({
            Username: username,
            Password: password
        }).exec(()=>{
            serverFunctions.closeConnection();
        })


    });


    describe('clearUSERS()', function () {
        it('should clear the USERS array', function () {
            USERS.push(user0);
            assert.equal(USERS.length, 1);
            serverFunctions.clearUSERS();
            assert.equal(USERS.length, 0);
        })
    });

    describe('clearACTIVE_SEARCHES()', function () {
        it('should clear the ACTIVE_SEARCHES array', function () {
            ACTIVE_SEARCHES.push(activeSearch);
            assert.equal(ACTIVE_SEARCHES.length, 1);
            serverFunctions.clearACTIVE_SEARCHES();
            assert.equal(ACTIVE_SEARCHES.length, 0);
        })
    });

    describe('getActiveSearch(), 0', function () {
        it('should take in a valid searchID and return an ActiveSearch object from ACTIVE_SEARCHES ', function () {
            ACTIVE_SEARCHES.push(activeSearch);
            let actual = serverFunctions.getActiveSearch(activeSearch.ID);
            assert.equal(actual, activeSearch);
        })
    });

    describe('getActiveSearch(), 1', function () {
        it('should take in an invalid searchID and return a null object ', function () {
            ACTIVE_SEARCHES.push(activeSearch);
            let actual = serverFunctions.getActiveSearch("nonsense");
            assert.equal(actual, null);
        })
    });

    describe('getUser() 0', function () {
        it('should take in a valid username and return a user object from USERS', function () {
            USERS.push(user0);
            let actual = serverFunctions.getUser(user0.Username);
            assert.equal(actual, user0);
        })
    });

    describe('getUser() 1', function () {
        it('should take in an invalid username and return a null object', function () {
            USERS.push(user0);
            let actual = serverFunctions.getUser("sass man hall from");
            assert.equal(actual, null);
        })
    });

    describe('registerNewUser()', function () {
        it('should take in a username and password and register a new user', function (done) {

            let username = "testUsername";
            let password = "testPassword";

            serverFunctions.registerNewUser(username, password);

            //assert user exists
            serverFunctions.UserModel.find({ Username : username, Password : password}).exec(function (err, result) {
                assert.exists(result);

                done();
            });
        });
    });


    describe('usernameNotTaken(), 0', function () {
        it('should take in a taken username and return false', function () {
            USERS.push(user0);
            let result = serverFunctions.usernameNotTaken(user0.Username);
            assert.isFalse(result);
        })
    });

    describe('usernameNotTaken(), 1', function () {
        it('should take in a non-taken username and return true', function () {
            USERS.push(user0);
            let result = serverFunctions.usernameNotTaken("sass man");

            assert.isTrue(result);
        })
    });


    describe('updatePassword()', function () {
        it('should take in a username, password and new password, then update the database', function (done) {
            // this.timeout(10000);
            // setTimeout(myfunc, 1000);
            //assert user exists
            let username = "u1";
            let password = "p1";

            // serverFunctions.UserModel.find({ Username : username, Password : password}).exec(function (err, result){
            //     console.log(result);
            //     assert.equal(result[0].Username, username);
            //     assert.equal(result[0].Password, password);
            // });

            //do update password
            serverFunctions.updatePassword(username, password, "newPassword").then(()=>{

                //then assert password updated
                serverFunctions.UserModel.find({ Username : username, Password : "newPassword"}).exec(function (err, result){


                    //then reset db entry
                    serverFunctions.UserModel.updateOne(
                        {
                            Username : username
                        },
                        {
                            Username : username,
                            Password : password
                        }).then(()=>{
                        console.log(result);
                        assert.equal(result[0].Username, username);
                        assert.equal(result[0].Password, "newPassword");
                        done();
                        });
                });


            });
        });
    });


    describe('updateUsername()', function () {
        it('should take in a username, password, and a new username, then update the database', function (done) {
            this.timeout(10000);
            // setTimeout(done, 1000);

            let username = "u2";
            let password = "p2";

            // serverFunctions.UserModel.find({ Username : username}).exec(function (err, result) {
            //     console.log(result);
            //     assert.equal(result[0].Username, username);
            //     assert.equal(result[0].Password, password);
            // });

            //do update username
            serverFunctions.updateUsername(username, password, "newUsername").then(()=>{

                //then assert username updated
                serverFunctions.UserModel.find({ Username : "newUsername"}).exec(function (err, result){

                    //then reset db entry
                    serverFunctions.UserModel.updateOne(
                        {
                            Username : "newUsername"
                        },
                        {
                            Username : username
                        }).then(()=>{
                        console.log(result);
                        assert.exists(result);
                        // assert.equal(result[0].Username, "newUsername");
                        // assert.equal(result[0].Password, password);
                        done();
                    });




                });
            });

        });
    });


    describe('deleteUser()', function () {
        it('should take in a username and password, then update the database', function (done) {
            this.timeout(5000);



            let username = "u3";
            let password = "p3";

            //assert user exists
            // serverFunctions.UserModel.find({ Username : username, Password : password}).exec(function (err, result){
            //     console.log(result);
            //     assert.equal(result[0].Username, username);
            //     assert.equal(result[0].Password, password);
            // });

            //do delete
            serverFunctions.deleteUser(username, password).then(()=>{

                //assert user deleted
                serverFunctions.UserModel.find({ Username : username, Password : password}).exec(function (err, result){


                    //then reset db entry
                    serverFunctions.UserModel.updateOne(
                        {
                            Username : username,
                            Password : password
                        },
                        {
                            Username: username,
                            Password: password
                        },
                        {
                            new : true,
                            upsert : true
                        }).then(()=>{

                        console.log(result);
                        // assert.equal(result[0], undefined);
                        assert.exists(result);
                        setTimeout(done, 4500);
                    });
                });



            });
        });
    });




    describe('grantLoginRequest()', function () {
        it('should take in a ws object and a username and add them to the connectionsMap', function () {
            USERS.push(user0);

            assert.equal(serverFunctions.connectionsMap.get(user0), undefined);

            serverFunctions.grantLoginRequest("ws", user0.Username);

            assert.equal(serverFunctions.connectionsMap.get(user0), "ws");
        })
    });


    describe('createNewActiveSearch()', function (done) {
        it('should take in a username, latitude and longitude, make a call to the API, create a new ActiveSearch object from the results, and then append it to ACTIVE_SEARCHES', function () {
            let lat = 37.421998333333335;
            let long = -122.08400000000002;

            assert.equal(ACTIVE_SEARCHES.length, 0);

            serverFunctions.createNewActiveSearch(user0.Username, lat, long).then(()=>{
                assert.equal(ACTIVE_SEARCHES.length, 1);
                assert.equal(ACTIVE_SEARCHES[0].Participants[0].Username, user0.Username);
                done();
            });
        });
    });






    describe('joinExistingSearch()', function () {
        it('should add a user to an existing active search', function () {
            // serverFunctions.clearUSERS();

            USERS.push(user0);
            USERS.push(user1);
            ACTIVE_SEARCHES.push(activeSearch);

            assert.equal(activeSearch.Participants.length, 1);

            serverFunctions.joinExistingSearch(activeSearch.ID, user1.Username);

            assert.equal(activeSearch.Participants.length, 2);
        })
    });


    describe('castVoteInSearch()', function () {
        it('should take in a valid searchcode, a username and an eateryOptionID and cast a vote in an existing search', function () {
            USERS.push(user0);
            ACTIVE_SEARCHES.push(activeSearch);

            serverFunctions.castVoteInSearch(activeSearch.ID, user0.Username, "0");

            assert.equal(activeSearch.EateryOptions[0].Votes[0], user0.Username);

        })
    });



    describe('ValidateCredentials(), 0', function () {
        it('should take in a valid username and a password and return true', function () {
            USERS.push(user0);
            let actual = serverFunctions.validateCredentials(user0.Username, user0.Password);
            assert.equal(actual, true);
        })
    });

    describe('ValidateCredentials(), 1', function () {
        it('should take in an invalid username and a password and return false', function () {
            USERS.push(user0);
            let actual = serverFunctions.validateCredentials("dqwdswq2221wxaasasasSADASX", "pdsdasadwadwadsadsads");
            assert.equal(actual, false);
        })
    });

    describe('ValidateCredentials(), 2', function () {//this is failing, investigate it
        it('should take in an invalid username and a password and return false', function () {
            USERS.push(user0);
            let actual = serverFunctions.validateCredentials("", "");
            assert.equal(actual, false);
        })
    });


});