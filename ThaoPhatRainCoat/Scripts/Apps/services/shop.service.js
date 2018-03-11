'use strict';
appShopPage.service('shopService', function ($http) {
    //get all products
    this.getAllProducts = function () {
        var response = $http({
            method: 'POST',
            url: '/Shop/GetAllProducts',
            data: {}
        });
        return response;
    },

    this.getProductsOrderBy = function (keyOrder) {
        var response = $http({
            method: 'GET',
            url: '/Shop/GetProductsOrderBy',
            data: {
                orderKey: keyOrder
            }
        });
        return response;
    }

    //this.createNewProduct = function (product) {
    //    var response = $http({
    //        method = 'POST',
    //        url: '/Shop/CreateNewProduct',
    //        data: {
    //            product: product
    //        }
    //    });
    //    return response;
    //},

    //this.updateProduct = function (product) {
    //    var response = $http({
    //        method = 'POST',
    //        url: '/Shop/UpdateProduct',
    //        data: {
    //            product: product
    //        }
    //    });
    //    return response;
    //}
});