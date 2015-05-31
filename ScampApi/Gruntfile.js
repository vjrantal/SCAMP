var path = require('path');

module.exports = function (grunt) {
    require('load-grunt-tasks')(grunt);

    grunt.initConfig({
        shell: {
            restore: {
                command: 'dnu restore'
            },
            build: {
                command: 'dnu build'
            },
            run: {
                command: 'dnx . web',
                options: {
                    async: true
                }
            },
            webdriver_update: {
                command: path.resolve('node_modules/.bin/webdriver-manager') + ' update'
            },
            protractor: {
                command: path.resolve('node_modules/.bin/protractor') + ' ' + path.join('test/conf.js')
            }
        },
        waitServer: {
            server: {
                options: {
                    url: 'http://localhost:44000',
                    timeout: 20 * 1000
                }
            },
        },
        protractor_webdriver: {
            start: {
                options: {
                    // Workaround for a similar issue than what is reported at:
                    // https://github.com/teerapap/grunt-protractor-runner/issues/111
                    keepAlive: true
                }
            }
        }
    });

    grunt.registerTask('build', ['shell:restore', 'shell:build']);
    grunt.registerTask('run', ['shell:run', 'waitServer']);
    grunt.registerTask('protractor-e2e-tests', ['shell:webdriver_update', 'protractor_webdriver', 'shell:protractor']);
    grunt.registerTask('test', ['run', 'protractor-e2e-tests']);
};
