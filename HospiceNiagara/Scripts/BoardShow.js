$(function () {


    $('#search').on('click', function (evt) {
        var $this = $(this);
        evt.preventDefault();

        var $search = 'SearchString='+ $('#txtSearch').val();

        var $url = $(this).attr('href');
        //if ($search.length > 0) {
        //    $url = $url + '/SearchString=' + $search;
        //}
        
        alert($url);
        $.ajax({
            method: $this.data('ajaxMethod').toUpperCase(),
            cache: false,
            url: $url,
            dataType: 'html',
            data: $search,
            success: function (data) {
                if (data.length == 0) {

                }
                else {
                    $('#someDiv').html(data);
                }
                

            }
        });

        //        //$.get($url, function (data) {
        //        //    $update.html(data);
        //        //});

    });

});