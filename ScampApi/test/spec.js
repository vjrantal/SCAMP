describe('SCAMP Home Page', function () {
    it('should have a title', function () {
        browser.get('http://localhost:44000/');

        expect(browser.getTitle()).toEqual('Scamp');
    });
});
