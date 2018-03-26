'use strict';
appSlideshowPage.controller('SlideshowController', function ($scope, $mdDialog, slideshowService, $interval, $timeout) {    
    //auto move next image after 5s
    $interval(function () {
        $('.moveNextCarousel').click();
    }, 5000);
    //the first method run when load shop page
    $scope.init = function () {
        slideshowService.getAllSlides().then(function (result) {
            result.data.slideshows.forEach(function (value, index) {
                value.ImagePathTemp = $scope.loadImageSrc(value.ImagePath);
            });
            $scope.shopModel = result.data;
            $scope.emptySlide = result.data.slideBE;
            //$scope.emptySlide.isEditable = true;
            $scope.allSlides = $scope.shopModel.slideshows;
            $scope.selectedSlide = $scope.allSlides.length > 0 ?
                $scope.allSlides[0] : angular.copy($scope.emptySlide);
            convertImgToBase64();
        });
    }

    //action select Slide
    $scope.selectSlide = function (element, slide) {
        $('.selected-slide').removeClass('selected-slide');
        $(element.currentTarget).addClass('selected-slide');
        $scope.selectedSlide = slide;
    }

    $scope.refreshPage = function () {
        $timeout(function () {
            $(".slide-grid").css("height", 200);
            $('.image-avatar').css('width', 200);
        }, 0);
    }

    $scope.loadImageSrc = function (imgPath) {
        return imgPath + '?' + new Date().getTime();
    }

    //isEditMode = true: edit
    //isEditMode = false: create
    $scope.showAdvanced = function (ev, isEditMode) {
        if ($scope.token == '')
            return;
        $mdDialog.show({
            controller: EditSlideDialogController,
            templateUrl: 'createUpdate.tmpl.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true,
            fullscreen: true,
            locals: {
                selectedSlide: isEditMode ? $scope.selectedSlide : angular.copy($scope.emptySlide),
                isEditMode: isEditMode
            }
        })
            .then(function (answer) {
                if (answer.answer) {
                    $scope.selectedSlide = answer.selectedSlide;
                    if (answer.selectedSlide.Id <= 0) {
                        $scope.allSlides.unshift($scope.selectedSlide);
                        slideshowService.createNewSlide($scope.selectedSlide).then(
                            function (result) {
                                $scope.allSlides[0] = result.data;
                                $scope.allSlides[0].ImagePathTemp = $scope.loadImageSrc($scope.allSlides[0].ImagePath);
                                $timeout(function () {
                                    $('.selected-slide').removeClass('selected-slide');
                                    $('.slide-grid').first().addClass('selected-slide');
                                    $('.image-avatar').first()
                                        .css('height', 200);
                                    $scope.$apply();
                                }, 10);
                            },
                            function (error) {
                                $scope.allSlides.shift();
                                alert("Thêm slide không thành công");
                            }
                        );
                    } else {
                        slideshowService.updateSlide($scope.selectedSlide).then(
                            function (result) {
                                $scope.selectedSlide.ImagePathTemp = $scope.loadImageSrc($scope.selectedSlide.ImagePath);
                            },
                            function (error) {
                                alert("Update slide không thành công");
                            }
                        );
                    }
                }
                if (answer.deleted) {
                    slideshowService.deleteSlide($scope.selectedSlide.Id).then(function (result) {
                        $scope.allSlides = $scope.allSlides.filter(function (obj) {
                            return obj.Id !== $scope.selectedSlide.Id;
                        });
                        $scope.selectedSlide = $scope.allSlides.length > 0 ?
                            $scope.allSlides[0] : null;
                    });
                }
            });
    };
    function EditSlideDialogController($scope, $mdDialog, selectedSlide, isEditMode) {
        $scope.selectedSlide = selectedSlide;
        $scope.isEditMode = isEditMode;
        $scope.cropper = null;
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer, selectedSlide) {
            var result = {
                deleted: false,
                answer: answer,
                selectedSlide: selectedSlide
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

        $scope.changeImage = function (element, Slide) {
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
                        aspectRatio: 16 / 9,
                        viewMode: 1,
                        crop: function (data) {
                            $("#crop-image")
                                .attr('src', this.cropper.getCroppedCanvas().toDataURL())
                                .attr('width', 200);
                            $scope.selectedSlide.ImageValue = this.cropper.getCroppedCanvas().toDataURL();
                        },
                        minContainerWidth: 100
                    });
                }
                var dataImage = reader.readAsDataURL(element.files[0]);
            }
        }
    }

    function getSlides() {
        slideshowService.getAllSlides().then(function (result) {
            $scope.allSlides = result.data.Slides;
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
                $scope.emptySlide.ImageValue = res;
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
