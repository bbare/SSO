import "babel-polyfill"
const emailAddress = 'winnmoo@gmail.com';


describe('No email provided', () =>{
    test('Send Reset Link Test', async () =>{
        let browser = await puppeteer.launch({
            headless: false,
            devtools: true,
            slowMo: 250
        });
        let page = await browser.newPage();

        page.emulate({
            viewport: {
                width: 800,
                height: 800
             },
             userAgent: ''
        });

        await page.goto('http://localhost:8080/#/sendresetlink');
        await page.waitForSelector('.sendLink')

        const html = await page.$eval('.sendLink', e => e.innerHTML);
        expect(html).tobe('Reset Password')

        browser.close
    }, 16000);
});
