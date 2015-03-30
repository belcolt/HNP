$(function () {
    //Img popular
    $('.show').on('click', function (evt) {
        var $this = $(this);
        evt.preventDefault();
        
        var i = 0;
        var $url = $(this).attr('href');

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

    //Modal populator
    $('.showModal').on('click', function (evt) {
        var $this = $(this);
        evt.preventDefault();

        var i = 0;
        var $url = $(this).attr('href');

        $.ajax({
            method: $this.data('ajaxMethod').toUpperCase(),
            cache: false,
            url: $url,
            dataType: 'html',
            success: function (data) {
                if (data.length > 0)
                {
                    $('#showModal').html(data);
                    $("div[tabindex='-1']:last").modal('show');
                }
            }
        });

        //$.get($url, function (data) {
        //    $update.html(data);
        //});
    });
});