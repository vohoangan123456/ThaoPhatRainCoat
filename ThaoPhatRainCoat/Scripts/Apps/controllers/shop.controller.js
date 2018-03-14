'use strict';
appShopPage.controller('shopController', function ($scope, shopService, $interval, $timeout) {
    //auto move next image after 5s
    $interval(function () {
        $('.moveNextCarousel').click();
    }, 5000);
    //the first method run when load shop page
    $scope.init = function () {
        $scope.isShowNew = true;
        $scope.searchKey = '';
        shopService.getAllProducts().then(function (result) {
            $scope.shopModel = result.data;
            $scope.emptyProduct = result.data.productBE;
            $scope.emptyProduct.isEditable = true;
            $scope.allProducts = $scope.shopModel.products;
            $scope.selectedProduct = $scope.allProducts.length > 0 ?
                $scope.allProducts[0] : $scope.emptyProduct;

            $timeout(function () {
                $(".product-grid").css("height", $(".product-grid").width() * 3/2);
            }, 10);
        });
    }
    //action add new product
    $scope.addNewProduct = function () {
        $scope.isShowNew = false;
        var newProduct = angular.copy($scope.emptyProduct);
        $scope.selectedProduct = newProduct;
        $scope.allProducts.unshift(newProduct);
        $timeout(function () {
            $('.selected-product').removeClass('selected-product');
            $('.product-grid').first().addClass('selected-product');
            $scope.$apply();
        }, 10);
    }
    //action accept create new product
    $scope.confirmSaveNewProduct = function () {
        $scope.isShowNew = true;
        shopService.createNewProduct($scope.allProducts[0]).then(function (result) {
            $scope.allProducts[0].Id = result.data;
            $scope.allProducts[0].isEditable = false;
            $scope.refreshPage();
        });
    }
    //action edit a product
    $scope.editProduct = function () {
    }
    //action delete a product
    $scope.deleteProduct = function () {
    }
    //action select product
    $scope.selectProduct = function (element, product) {
        $('.selected-product').removeClass('selected-product');
        $(element.currentTarget).addClass('selected-product');
        $scope.selectedProduct = product;
    }
    //action search products
    $scope.searchProduct = function () {
        //alert($scope.searchKey);
    }
    //select sort by criterion
    $scope.sortBy = function (element) {
        var criterion = $(element.currentTarget).attr('name');
        var newValueForButton = "Sorting by: " + $(element.currentTarget).find('span').html() + " <i class='fa fa-caret-down'></i>";
        $('#sort-criterion').html(newValueForButton);
        //handle the action sorting
    }
    $scope.refreshPage = function () {
        $timeout(function () {
            $scope.$apply();
        }, 0);
    }

    $scope.changeImage = function (element) {
        if (element.files && element.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $(element.previousElementSibling).attr('src', e.target.result);
            }
            reader.readAsDataURL(element.files[0]);
        }
    }

    function getProducts() {
        shopService.getProductsByCondition()
            .then(function (result) {
            });
    }
});
//$("#input").change(function () {
//    readURL(this);
//});

//function readURL(input) {
//    if (input.files && input.files[0]) {
//        var reader = new FileReader();

//        reader.onload = function (e) {
//            $('.image-avatar').attr('src', e.target.result);
//        }
//        reader.readAsDataURL(input.files[0]);
//    }
//}
$(function () {
    

    // start carrousel
    $('.carousel.carousel-slider').carousel({
        fullWidth: true,
        indicators: false
    });

    // move next carousel
    $('.moveNextCarousel').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $('.carousel').carousel('next');
    });

    // move prev carousel
    $('.movePrevCarousel').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $('.carousel').carousel('prev');
    });
});