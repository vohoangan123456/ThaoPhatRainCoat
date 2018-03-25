var appShopPage = angular.module("appShopPage", ['ngMaterial', 'ngMessages', 'material.svgAssetsCache']);
var appHomePage = angular.module("appHomePage", []);
var appAboutPage = angular.module("appAboutPage", []);
var appLoginPage = angular.module("appLoginPage", []);
var appSlideshowPage = angular.module("appSlideshowPage", ['ngMaterial', 'ngMessages', 'material.svgAssetsCache']);

/*------------------HELP ANGULARJS WORK WELL WITH BOOTSTRAP SELECT------------------  
---- Author: LinhNV 
---- To use, just inject angular-bootstrap-select module to your app, and add selectpicker directive to select element 
------------------------------------------------------------------------------------*/
var localStorageKey = {
    token: 'token'
};

function getLocalStorage(key) {
    return window.localStorage.getItem(key);
}
function setLocalStorage(key, value) {
    window.localStorage.setItem(key, value);
}
function removeLocalStorage(key) {
    window.localStorage.removeItem(key);
}
angular.module('angular-bootstrap-select', [])
    .directive('selectpicker', ['$parse', function ($parse) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.selectpicker($parse(attrs.selectpicker)());
                element.selectpicker('refresh');

                scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                    scope.$parent[attrs.ngModel] = newVal;
                    scope.$evalAsync(function () {
                        if (!attrs.ngOptions || /track by/.test(attrs.ngOptions)) element.val(newVal);
                        element.selectpicker('refresh');
                    });
                });

                scope.$on('$destroy', function () {
                    scope.$evalAsync(function () {
                        element.selectpicker('destroy');
                    });
                });
            }
        };
    }]);

/*------------------CAPTURE ON AJAX CALL EVENT FOR SHOWING LOADING SPINNER------------------  
---- Author: LinhNV 
---- To use, just inject loading-spinner module to your app then add loading backdrop with ng-show="isLoading" 
--------------------------------------------------------------------------------------------*/
angular.module('loading-spinner', [])
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('HttpInterceptor');
    }])
    .factory('HttpInterceptor', ['$rootScope', '$q', '$timeout', function ($rootScope, $q, $timeout) {
        return {
            'request': function (config) {
                $rootScope.isLoading = true;
                return config || $q.when(config);
            },
            'requestError': function (rejection) {
                return $q.reject(rejection);
            },
            'response': function (response) {
                $timeout(function () {
                    $rootScope.isLoading = false;       // hide loading screen after 300ms to prevent flash loading spinner
                }, 300);

                return response || $q.when(response);
            },
            'responseError': function (rejection) {
                /*...*/
                return $q.reject(rejection);
            }
        };
    }]);

