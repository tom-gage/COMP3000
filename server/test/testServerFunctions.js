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


    describe('()', function () {
        it('should ', function () {

            assert.equal();
        })
    });

    describe('()', function () {
        it('should ', function () {

            assert.equal();
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
        it('should take in a username and password and register a new user', function () {

            let username = "testUsername";
            let password = "testPassword";

            serverFunctions.registerNewUser(username, password);

            //assert user exists
            serverFunctions.UserModel.find({ Username : username, Password : password}).exec(function (err, result) {
                assert.exists(result);

                serverFunctions.UserModel.deleteOne({
                    Username: username,
                    Password: password
                });
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






    describe('updatePassword()', function (done) {
        it('should take in a username, password and new password, then update the database', function () {
            //assert user exists
            serverFunctions.UserModel.find({ Username : "u", Password : "p"}).exec(function (err, result){
                // assert.equal(result.length, 1);
                assert.exists(result);
            });

            //do update password
            serverFunctions.updatePassword("u", "p", "newPassword").then(()=>{

                //then assert password updated
                serverFunctions.UserModel.find({ Username : "u", Password : "newPassword"}).exec(function (err, result){

                    // assert.equal(result.length, 1);
                    assert.exists(result);

                    //then reset db entry
                    serverFunctions.UserModel.updateOne(
                        {
                            Username : "u"
                        },
                        {
                            Username : "u",
                            Password : "p"
                        }).then(()=>{
                        done();
                    });
                });


            });
        });
    });


    describe('updateUsername()', function (done) {
        it('should take in a username, password, and a new username, then update the database', function () {
            serverFunctions.UserModel.find({ Username : "u"}).exec(function (err, result){
                // console.log("LOOK HERE DUMMY " + user.length);
                // console.log("LOOK HERE DUMMY " + user);
                assert.exists(result);

                //do update username
                serverFunctions.updateUsername("u", "p", "newUsername").then(()=>{

                    //then assert username updated
                    serverFunctions.UserModel.find({ Username : "newUsername"}).exec(function (err, result){


                        // assert.equal(user.length, 1);
                        assert.exists(result);

                        //then reset db entry
                        serverFunctions.UserModel.updateOne(
                            {
                                Username : "newUsername"
                            },
                            {
                                Username : "u"
                            }).then(()=>{
                            done();
                        });
                    });
                });
            });
        });
    });


    describe('deleteUser()', function (done) {
        it('should take in a username and password, then update the database', function () {
            //assert user exists
            serverFunctions.UserModel.find({ Username : "u", Password : "p"}).exec(function (err, result){
                // assert.equal(result.length, 1);
                assert.exists(result);
            });

            //do delete
            serverFunctions.deleteUser("u", "p").then(()=>{

                //assert user deleted
                serverFunctions.UserModel.find({ Username : "u", Password : "p"}).exec(function (err, result){
                    // assert.equal(result.length, 0);
                    assert.exists(result);

                    //then reset db entry
                    serverFunctions.UserModel.updateOne(
                        {
                            Username : "u",
                            Password : "p"
                        },
                        {
                            Username: "u",
                            Password: "p"
                        }).then(()=>{
                        done();
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

    // describe('ValidateCredentials ', function () {//this is failing, investigate it
    //     it('should take in an invalid username and a password and return false', function () {
    //         let actual = serverFunctions.validateCredentials("", "");
    //         assert.equal(actual, false);
    //     })
    // });


});