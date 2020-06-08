const { series } = require('gulp');
const gulp = require('gulp');
const npmDist = require('gulp-npm-dist');
const rename = require('gulp-rename');

function CopyNpmDistributionFiles(callback){
    gulp.src(npmDist(), {base:'./node_modules/'})
        .pipe(rename(function(path) {
            path.dirname = path.dirname.replace(/\/dist/, '').replace(/\\dist/, '');
        }))
        .pipe(gulp.dest('./wwwroot/libs'));

    callback();
}

exports.build = series(CopyNpmDistributionFiles);