promien_kuli = 50;
szerokosc_wyciecia = 30;
wysokosc_wyciecia = 45;
glebokosc_wyciecia = promien_kuli * 2 + 10;
$fn = 100;

minkowski() {
    difference() {
        sphere(r = promien_kuli - 2);
        translate([0, 0, promien_kuli - wysokosc_wyciecia/2 + 2])
            cube([szerokosc_wyciecia, glebokosc_wyciecia, wysokosc_wyciecia], center = true);
        translate([0, 0, -promien_kuli + wysokosc_wyciecia/2 - 2])
            cube([szerokosc_wyciecia, glebokosc_wyciecia, wysokosc_wyciecia], center = true);
    }
    sphere(r = 2, $fn = 20);
}