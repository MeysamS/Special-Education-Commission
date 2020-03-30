(function ($) {
    $.validator.setDefaults({
        showErrors: function (errorMap, errorList) {
            //اگر المانی معتبر است نیاز به نمایش تول‌تیپ ندارد
            $("." + this.settings.validClass).tooltip("destroy");
            //افزودن تول‌تیپ‌ها
            //افزودن تول‌تیپ‌ها
            for (var i = 0; i < errorList.length; i++) {
                var error = errorList[i];
                $(error.element).tooltip({
                    position: 'top',
                    content: error.message,
                    onShow: function () {
                        $(this).tooltip('tip').css({
                            color: "#000",
                            borderColor: "#CC9933",
                            backgroundColor: "#FFFFCC"
                        });
                    }
                });
                $(error.element).tooltip('show');
            }
        },
        // همانند قبل برای رنگی کردن کل ردیف در صورت عدم اعتبار سنجی و برعکس
        highlight: function (element, errorClass, validClass) {
            if (element.type === 'radio') {
                this.findByName(element.name).addClass(errorClass).removeClass(validClass);
            } else {
                $(element).addClass(errorClass).removeClass(validClass);
                $(element).closest('.form-group').removeClass('success').addClass('has-error');
            }
            $(element).addClass('tooltip-darkorange');
            $(element).trigger('highlited');
        },
        unhighlight: function (element, errorClass, validClass) {
            if (element.type === 'radio') {
                this.findByName(element.name).removeClass(errorClass).addClass(validClass);
            } else {
                $(element).removeClass(errorClass).addClass(validClass);
                $(element).closest('.form-group').removeClass('has-error').addClass('success');
            }
            $(element).addClass('tooltip-darkorange');
            $(element).trigger('unhighlited');
        }

    });

})(jQuery);
