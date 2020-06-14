$( document ).ready( function () {
    $( "#search" ).keyup( function () {
        _this = this;
        $.each( $( "#eventsTable tbody tr" ), function () {
            console.log( $( this ).children( 'td:first-child' ).text())
            if ( $( this ).children( 'td:first-child' ).text().toLowerCase().indexOf($( _this ).val().toLowerCase()) === -1)
                $( this ).hide();
            else
                $( this ).show();
        });
    });
});