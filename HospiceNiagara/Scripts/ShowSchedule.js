$(function () {

    $('.show').on('click', function (evt) {
        var $this = $(this);
        evt.preventDefault();
        
        var i = 0;
        var $update = $this.attr('data-ajax-update'),
            $url = $(this).attr('href');
        var div = $update;

        $.ajax({
            method: $this.data('ajaxMethod').toUpperCase(),
            cache: false,
            url: $url,
            dataType: 'html',
            success: function (data) {
                $('#someDiv').html(data);
                
            }
        });

        //$.get($url, function (data) {
        //    $update.html(data);
        //});

    });

});