$(function () {

    $('.sel-sched').on('change', function (evt) {
        var $this = $(this);
        if ($this.val() != "") {


            evt.preventDefault();


            
               var $url = '/Schedules/ShowSched/' + $this.val();
            


            $.ajax({
                method: 'POST',
                cache: false,
                url: $url,
                dataType: 'html',
                success: function (data) {
                    $('#someDiv').html(data);

                }
            });
        }
        //$.get($url, function (data) {
        //    $update.html(data);
        //});

    });

});