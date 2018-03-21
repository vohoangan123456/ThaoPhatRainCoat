'use strict';
appShopPage.controller('shopController', function ($scope, $mdDialog, shopService, $interval, $timeout) {
    //auto move next image after 5s
    $interval(function () {
        $('.moveNextCarousel').click();
    }, 5000);
    //the first method run when load shop page
    $scope.init = function () {
        convertImgToBase64();
        $scope.isShowNew = true;
        $scope.searchKey = '';
        $scope.productSize = {};
        $scope.status = '  ';
        $scope.customFullscreen = false;
        shopService.getAllProducts().then(function (result) {
            $scope.shopModel = result.data;
            $scope.emptyProduct = result.data.productBE;
            $scope.emptyProduct.isEditable = true;
            $scope.allProducts = $scope.shopModel.products;
            $scope.selectedProduct = $scope.allProducts.length > 0 ?
                $scope.allProducts[0] : angular.copy($scope.emptyProduct);

            $timeout(function () {
                $(".product-grid").css("height", $(".product-grid").width() * 5 / 3);
                $scope.productSize = {
                    height: $(".product-grid").height(),
                    width: $(".product-grid").width()
                };
                $('.image-avatar').css('width', $(".product-grid").width())
                    .css('height', $(".product-grid").width());
            }, 10);
        });
    }
    //action add new product
    //$scope.addNewProduct = function () {
    //    $scope.isShowNew = false;
    //    var newProduct = angular.copy($scope.emptyProduct);
    //    $scope.selectedProduct = newProduct;
    //    $scope.allProducts.unshift(newProduct);
    //    $timeout(function () {
    //        $('.selected-product').removeClass('selected-product');
    //        $('.product-grid').first().addClass('selected-product');
    //        $scope.$apply();
    //    }, 10);
    //}
    ////action accept create new product
    //$scope.confirmSaveNewProduct = function () {
    //    $scope.isShowNew = true;
    //    shopService.createNewProduct($scope.allProducts[0]).then(function (result) {
    //        $scope.allProducts[0].Id = result.data;
    //        $scope.allProducts[0].isEditable = false;
    //        $scope.refreshPage();
    //    });
    //}
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

    $scope.changeImage = function (element, product) {
        if (element.files && element.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $(element.previousElementSibling)
                    .attr('src', e.target.result)
                    .attr('height', $scope.productSize.width)
                    .attr('width', $scope.productSize.width);
                $scope.selectedProduct.ImageValue = e.target.result;
                //$timeout(function () {
                //    $scope.$apply();
                //}, 10);
            }
            var dataImage = reader.readAsDataURL(element.files[0]);
        }
    }

    //isEditMode = true: edit
    //isEditMode = false: create
    $scope.showAdvanced = function (ev, isEditMode) {
        $mdDialog.show({
            controller: DialogController,
            templateUrl: 'createUpdate.tmpl.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true,
            fullscreen: true,
            locals: {
                selectedProduct: isEditMode ? $scope.selectedProduct : angular.copy($scope.emptyProduct),
                isEditMode: isEditMode,
                popupSize: $scope.productSize
            }
        })
        .then(function (answer, selectedProduct) {
            if (answer) {
                $scope.selectedProduct = selectedProduct;
                $scope.allProducts.unshift($scope.selectedProduct);
            }
        });
    };
    function DialogController($scope, $mdDialog, selectedProduct) {
        $scope.selectedProduct = selectedProduct;
        $scope.productSize = {
            height: 800,
            width: 400
        };
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
        };
        $scope.changeImage = function (element, product) {
            if (element.files && element.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $(element.previousElementSibling)
                        .attr('src', e.target.result)
                        .attr('height', $scope.productSize.width)
                        .attr('width', $scope.productSize.width);
                    $scope.selectedProduct.ImageValue = e.target.result;
                }
                var dataImage = reader.readAsDataURL(element.files[0]);
            }
        }
    }

    function getProducts() {
        shopService.getProductsByCondition()
            .then(function (result) {
            });
    }

    function convertImgToBase64() {
        var xhr = new XMLHttpRequest();
        xhr.open("GET", "Content/images/default-image.jpg", true);
        xhr.responseType = "blob";
        xhr.onload = function (e) {
            console.log(this.response);
            var reader = new FileReader();
            reader.onload = function (event) {
                var res = event.target.result;
                $scope.emptyProduct.ImageValue = res;
            }
            var file = this.response;
            reader.readAsDataURL(file)
        };
        xhr.send();
    }
});

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