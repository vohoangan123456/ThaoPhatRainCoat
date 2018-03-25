'use strict';
appLoginPage.service('loginService', function ($http) {
    //get all products
    this.login = function (account, pwd) {
        var response = $http({
            method: 'POST',
            url: '/Account/Login',
            data: {
                account: account,
                password: pwd
            }
        });
        return response;
    };
});