const assert = require('chai').assert;
const ServerFunctions = require('../ServerFunctions');
const EateryOption = require('../objects/EateryOption');
const User = require('../objects/User');
const DB = require('../DB');

describe('hooks', function () {

    let serverFunctions;
    let eateryOption;
    let user;
    let UserModel;

    // global.USERS = [];
    // global.CONNECTED_USERS = [];
    // global.ACTIVE_SEARCHES = [];
    //

    //
    // UserModel = DB.getUserModel();
    // UserModel.find({}, function (err, users) {
    //     global.USERS = users;
    // });


    before(function (done){
        serverFunctions = new ServerFunctions();

        serverFunctions.initConnection().then(()=>{//this thing ensures that the tests wait until the DB's connected before running, don't ask me how it works
            done();
        })
    });

    beforeEach(function () {
        eateryOption = new EateryOption("123", "Five Guys", "Burgers and fries", "10", "123");
        user = new User("testUser", "password", "123");
    });

    describe('ValidateCredentials ', function () {
        it('should take in a valid username and a password and return true', function () {
            let actual = serverFunctions.validateCredentials("u", "p");
            assert.equal(actual, true);
        })
    });
});