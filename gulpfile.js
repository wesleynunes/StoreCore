var gulp = require('gulp');
var cssmin = require('gulp-cssmin');

gulp.task('js', function () {
    return gulp.src([
        './node_modules/bootstrap/dist/js/bootstrap.min.js',
        './node_modules/bootstrap/dist/js/bootstrap.bundle.min.js',
        './node_modules/jquery/dist/jquery.min.js',        
        './node_modules/jquery-validation/dist/jquery.validate.min.js',
        './node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js',
        './node_modules/glyphicons/glyphicons.js',
        './node_modules/qrcode.js/src/qrcode.js',  
        //'./Assets/js/site.js',
        './Assets/js/fontawesome.min.js',
        './Assets/js/all.min.js',
    ])
        //.pipe(browserSync.stream())
        .pipe(gulp.dest('wwwroot/js/'));
});

gulp.task('css', function () {
    return gulp.src([        
        //'./Assets/css/site.css',
        './Assets/css/fontawesome.min.css',
        './node_modules/bootstrap/dist/css/bootstrap.min.css',  
        //painel admin
        './Assets/admin/css/simple-sidebar.css',        
    ])
        .pipe(cssmin())    
        .pipe(gulp.dest('wwwroot/css/'));
});

gulp.task('images', function () {
    return gulp.src([
        './Assets/images/favicon.ico',      
    ])
        //.pipe(browserSync.stream())        
        .pipe(gulp.dest('wwwroot/'));
});
