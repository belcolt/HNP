$(function () {

    $("#resCatDD").on("change", function () {
       getSubs($(this).val());
    });

    function getSubs(catID) {
        $.getJSON("/Resources/LoadSubcategories","catID="+catID,function (data) {
            $(".subCatGen").remove();
            $.each(data, function (i, item) {
                $("#resSubCatDD").append("<option class=subCatGen value=" + data[i].ID + ">" + data[i].Name + "</option>");
            });
        });
    };

    $("#btnRSVP").click(function () {

    });
});