'use strict';
appShopPage.controller('shopController', function ($scope, $mdDialog, shopService, $interval, $timeout) {
    //auto move next image after 5s
    $interval(function () {
        $('.moveNextCarousel').click();
    }, 5000);
    //the first method run when load shop page
    $scope.init = function () {
        $scope.token = globalResources.token;
        $scope.searchKey = '';
        $scope.sortByCriterion = 1; //[1,2,3,4] = [price heigh-low, low-heigh, name a-z, z-a]
        $scope.productSize = {};
        $scope.status = '  ';
        $scope.customFullscreen = false;
        //handleDeviceSize();
        shopService.getAllProducts().then(function (result) {
            $scope.shopModel = result.data;
            $scope.emptyProduct = result.data.productBE;
            //$scope.emptyProduct.isEditable = true;
            $scope.allProducts = $scope.shopModel.products;
            $scope.selectedProduct = $scope.allProducts.length > 0 ?
                $scope.allProducts[0] : angular.copy($scope.emptyProduct);
            convertImgToBase64();
            $timeout(function () {
                $(".product-grid").css("height", $(".product-grid").width() * 5 / 3);
                $scope.productSize = {
                    height: $(".product-grid").height(),
                    width: $(".product-grid").width()
                };
                $('.image-avatar').css('width', $(".product-grid").width()- 15);
            }, 10);
        });
    }
    
    //action select product
    $scope.selectProduct = function (element, product) {
        $('.selected-product').removeClass('selected-product');
        $(element.currentTarget).addClass('selected-product');
        $scope.selectedProduct = product;
    }
    //action search products
    $scope.searchProduct = function () {
        getProducts();
    }
    //select sort by criterion
    $scope.sortBy = function (element) {
        $scope.sortByCriterion = $(element.currentTarget).attr('name');
        var newValueForButton = "Sorting by: " + $(element.currentTarget).find('span').html() + " <i class='fa fa-caret-down'></i>";
        $('#sort-criterion').html(newValueForButton);
        //handle the action sorting
        getProducts();
    }
    $scope.refreshPage = function () {
        $timeout(function () {
            $(".product-grid").css("height", $(".product-grid").width() * 5 / 3);
            $('.image-avatar').css('width', $(".product-grid").width() - 15);
        }, 0);
    }

    //show edit for slideshow
    $scope.showSlideshowEdit = function (ev) {
        window.location.href = '/Shop/Slideshow';
        //$mdDialog.show({
        //    controller: SlideshowDialogController,
        //    templateUrl: 'editSlideshow.tmpl.html',
        //    parent: angular.element(document.body),
        //    targetEvent: ev,
        //    clickOutsideToClose: true,
        //    fullscreen: true,
        //    locals: {
        //        listSlideshow: angular.copy($scope.allProducts)
        //    }
        //}).then(function (answer) {
        //    if (answer) {
        //    }
        //});
    }
    function SlideshowDialogController($scope, $mdDialog, listSlideshow) {
        $scope.listSlideshow = listSlideshow;
        $scope.cropper = null;
        $scope.selectedSlide = null;
        $scope.selectedSlideOrigin = null;
        //function
        $scope.cancel = function () {
            $scope.selectedSlide = angular.copy($scope.selectedSlideOrigin);
            $scope.cropper.destroy();
            $scope.cropper = null;
            $(".original-image-slideshow").css('display', 'none');
            $("#crop-image-" + $scope.selectedSlide.Id)
                .attr('src', $scope.selectedSlide.ImageValue)
                .attr('width', 200);
        };
        $scope.save = function () {
            $scope.cropper.destroy();
            $scope.cropper = null;
            $(".original-image-slideshow").css('display', 'none');
        };
        $scope.delete = function () {
            var result = {
                deleted: true,
                answer: null
            }
            $mdDialog.hide(result);
        };
        $scope.close = function () {
            var result = {
                answer: false
            }
            $mdDialog.hide();
        }
        $scope.changeImage = function (element) {
            if (element.files && element.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    if ($scope.cropper != null) {
                        $scope.cropper.destroy();
                        $scope.cropper = null;
                    }
                    $("#original-image-" + $scope.selectedSlide.Id)
                        .attr('src', e.target.result);
                    $scope.cropper = new Cropper(document.querySelector("#original-image-" + $scope.selectedSlide.Id), {
                        scalable: false,
                        aspectRatio: 16 / 9,
                        viewMode: 1,
                        crop: function (data) {
                            $("#crop-image-" + $scope.selectedSlide.Id)
                                .attr('src', this.cropper.getCroppedCanvas().toDataURL())
                                .attr('width', 200);
                            $scope.selectedSlide.ImageValue = this.cropper.getCroppedCanvas().toDataURL();
                        },
                        minContainerWidth: 200
                    });
                }
                var dataImage = reader.readAsDataURL(element.files[0]);
            }
        }
        $scope.clickImage = function (element, slide) {
            $('.slideshow-popup-selected').removeClass('slideshow-popup-selected');
            $(element.currentTarget).addClass('slideshow-popup-selected');
            $scope.selectedSlide = slide;
            $scope.selectedSlideOrigin = angular.copy(slide);
        }
    }

    //isEditMode = true: edit
    //isEditMode = false: create
    $scope.showAdvanced = function (ev, isEditMode) {
        if ($scope.token == '')
            return;
        $mdDialog.show({
            controller: EditProductDialogController,
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
        .then(function (answer) {
            if (answer.answer) {
                $scope.selectedProduct = answer.selectedProduct;
                if (answer.selectedProduct.Id <= 0) {
                    $scope.allProducts.unshift($scope.selectedProduct);
                    shopService.createNewProduct($scope.selectedProduct).then(function (result) {
                        $scope.allProducts[0].Id = result.data;
                        $timeout(function () {
                            $('.selected-product').removeClass('selected-product');
                            $('.product-grid').first().addClass('selected-product');
                            $('.image-avatar').first()
                                .css('width', $(".product-grid").width() - 15);
                            $scope.$apply();
                        }, 10);
                    });
                } else {
                    shopService.updateProduct($scope.selectedProduct).then(function (result) {});
                }
            }
            if (answer.deleted)
            {
                shopService.deleteProduct($scope.selectedProduct.Id).then(function (result) {
                    $scope.allProducts = $scope.allProducts.filter(function (obj) {
                        return obj.Id !== $scope.selectedProduct.Id;
                    });
                    $scope.selectedProduct = $scope.allProducts.length > 0 ?
                        $scope.allProducts[0] : null;
                });
            }
        });
    };
    function EditProductDialogController($scope, $mdDialog, selectedProduct, isEditMode) {
        $scope.selectedProduct = selectedProduct;
        $scope.isEditMode = isEditMode;
        $scope.cropper = null;
        $scope.productSize = {
            height: 800,
            width: 230
        };
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer, selectedProduct) {
            var result = {
                deleted: false,
                answer: answer,
                selectedProduct: selectedProduct
            };
            $mdDialog.hide(result);
        };

        $scope.delete = function () {
            var result = {
                deleted: true,
                answer: null
            }
            $mdDialog.hide(result);
        };

        $scope.changeImage = function (element, product) {
            if (element.files && element.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    if ($scope.cropper != null) {
                        $scope.cropper.destroy();
                        $scope.cropper = null;
                    }
                    $("#original-image")
                        .attr('src', e.target.result);
                    $scope.cropper = new Cropper(document.querySelector("#original-image"), {
                        scalable: false,
                        aspectRatio: 12 / 16,
                        viewMode: 1,
                        crop: function (data) {
                            $("#crop-image")
                                .attr('src', this.cropper.getCroppedCanvas().toDataURL())
                                .attr('width', 200);
                            $scope.selectedProduct.ImageValue = this.cropper.getCroppedCanvas().toDataURL();
                        },
                        minContainerWidth: 100
                    });
                }
                var dataImage = reader.readAsDataURL(element.files[0]);
            }
        }
    }

    function getProducts() {
        shopService.getProductsByCondition($scope.searchKey, $scope.sortByCriterion).then(function (result) {
            $scope.allProducts = result.data;
            $scope.refreshPage();
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