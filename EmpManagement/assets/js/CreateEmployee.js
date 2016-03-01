function onPageLoad() {
    $(document).on("click", "#formLogin", function () {

        //     $('.hd').hide();

        $('#formId :input').each(function (index, element) {
            if (element.value === '') {
                
                $($(".hd")[i]).html("Please enter your Email");
                $($(".hd")[0]).show();
            }
        });
    });

}
$(function () {
    $('.hd').hide();
    onPageLoad();
});