'use strict';
appShopPage.controller('shopController', function ($scope, shopService) {

    $scope.init = function () {
        shopService.getAllProducts().then(function (result) {
            $scope.shopModel = result.data;
            $scope.allProducts = $scope.shopModel.products;
        });
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
    $(".product-grid").css("height", $(".product-grid").width());
    var dropdown = document.getElementsByClassName("sort-by");
    var i;

    for (i = 0; i < dropdown.length; i++) {
        dropdown[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var dropdownContent = this.nextElementSibling;
            if (dropdownContent.style.visibility === "hidden" || dropdownContent.style.visibility === "") {
                dropdownContent.style.visibility = "visible";
            } else {
                dropdownContent.style.visibility = "hidden";
            }
        });
    }
});
//function doAnimations(elems) {
//    //Cache the animationend event in a variable
//    var animEndEv = 'webkitAnimationEnd animationend';

//    elems.each(function () {
//        var $this = $(this),
//            $animationType = $this.data('animation');
//        $this.addClass($animationType).one(animEndEv, function () {
//            $this.removeClass($animationType);
//        });
//    });
//}

//Variables on page load 
//var $myCarousel = $('#carousel-example-generic'),
//    $firstAnimatingElems = $myCarousel.find('.item:first').find("[data-animation ^= 'animated']");

////Initialize carousel 
//$myCarousel.carousel();

////Animate captions in first slide on page load 
//doAnimations($firstAnimatingElems);

////Pause carousel  
//$myCarousel.carousel('pause');


////Other slides to be animated on carousel slide event 
//$myCarousel.on('slide.bs.carousel', function (e) {
//    var $animatingElems = $(e.relatedTarget).find("[data-animation ^= 'animated']");
//    doAnimations($animatingElems);
//});
//$('#carousel-example-generic').carousel({
//    interval: 3000,
//    pause: "false"
//});