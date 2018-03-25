'use strict';
appSlideshowPage.service('slideshowService', function ($http) {
    //get all slides
    this.getAllSlides = function () {
        var response = $http({
            method: 'POST',
            url: '/Shop/GetAllSlides',
            data: {}
        });
        return response;
    };

    this.createNewSlide = function (slide) {
        var response = $http({
            method: 'post',
            url: '/Shop/CreateNewSlide',
            data: {
                slide: slide
            }
        });
        return response;
    };

    this.updateSlide = function (slide) {
        var response = $http({
            method: 'post',
            url: '/Shop/UpdateSlide',
            data: {
                slide: slide
            }
        });
        return response;
    };

    this.deleteSlide = function (slideId) {
        var response = $http({
            method: 'post',
            url: '/Shop/DeleteSlide',
            data: {
                slideId: slideId
            }
        });
        return response;
    };
});