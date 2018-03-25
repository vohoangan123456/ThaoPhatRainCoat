'use strict';
appLoginPage.controller('loginController', function ($scope, loginService, $timeout) {
    $scope.username = '';
    $scope.password = '';
    $scope.error = '';
    $scope.init = function () {
        console.info('hihi');
    }
    $scope.formSubmit = function () {
        $scope.error = '';
        loginService.login($scope.username, $scope.password).then(function (result) {
            if (result.data.isSuccess) {
                window.location.href = '/Shop';
            }
            else {
                $scope.error = "Login fail! please call 01638705850 for supports";
            }
        });
    }
});