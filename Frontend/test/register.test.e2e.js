// Deps
const puppeteer = require('puppeteer');
const { expect } = require('chai');

// Constants
const baseURL = process.env.BASE_URL || 'http://localhost:8080/#';
const registrationURL = `${baseURL}/register`;
const TYPE_DELAY = 0;

let animationTimeout = () => new Promise(resolve => setTimeout(resolve, 500))
let randomNum = (start, end) => Math.floor((Math.random() + start) * end)
let randomEmail = () => `${randomNum(1, 99999999)}@${randomNum(1, 99999999)}.com`

let fillReg = async page => {
    await page.type('#email', randomEmail(), {delay: TYPE_DELAY})
    await page.type('#password', 'G3jKk.jf0D](pl;cVa9j', {delay: TYPE_DELAY})
    await page.type('#confirm', 'G3jKk.jf0D](pl;cVa9j', {delay: TYPE_DELAY})

    await page.click('#dob')
    
    let yearSelector = 'ul.v-date-picker-years li:last-child'
    await page.waitForSelector(yearSelector)
    await animationTimeout()
    await page.click(yearSelector)
    
    let monthSelector = '.v-date-picker-table--month tr:last-child td:last-child button'
    await page.waitForSelector(monthSelector)
    await animationTimeout()
    await page.click(monthSelector)
    
    let daySelector = '.v-date-picker-table--date tr:last-child td:last-child button'
    await page.waitForSelector(daySelector)
    await animationTimeout()
    await page.click(daySelector)

    await page.type('#city', 'Los Angeles', {delay: TYPE_DELAY})
    await page.type('#state', 'California', {delay: TYPE_DELAY})
    await page.type('#country', 'United States', {delay: TYPE_DELAY})

    await page.click('#securityq1')
    await animationTimeout()
    await page.click('.menuable__content__active .v-select-list .v-list > div:first-child')
    await page.type('#securitya1', 'Answer 1.', {delay: TYPE_DELAY})

    await page.click('#securityq2')
    await animationTimeout()
    await page.click('.menuable__content__active .v-select-list .v-list > div:first-child')
    await page.type('#securitya2', 'Answer 2.', {delay: TYPE_DELAY})
    
    await page.click('#securityq3')
    await animationTimeout()
    await page.click('.menuable__content__active .v-select-list .v-list > div:first-child')
    await page.type('#securitya3', 'Answer 3.', {delay: TYPE_DELAY})
}

describe('registration', () => {
    let browser
    before(async () => {
        browser = await puppeteer.launch({ headless: false });
    })
    after(async () => {
        await browser.close()
    })

    describe('interface fields', () => {
        let page
        before(async () => {
            page = await browser.newPage();
            await page.goto(registrationURL);
            await page.waitForSelector('main h1');
        })
        after(async () => {
            await page.close()
        })

        it('renders expected inputs', async () => {
            expect(await page.$('#email')).to.not.be.null
            expect(await page.$('#password')).to.not.be.null
            expect(await page.$('#confirm')).to.not.be.null

            expect(await page.$('#city')).to.not.be.null
            expect(await page.$('#state')).to.not.be.null
            expect(await page.$('#country')).to.not.be.null

            expect(await page.$('#securityq1')).to.not.be.null
            expect(await page.$('#securitya1')).to.not.be.null
            expect(await page.$('#securityq2')).to.not.be.null
            expect(await page.$('#securitya2')).to.not.be.null
            expect(await page.$('#securityq3')).to.not.be.null
            expect(await page.$('#securitya3')).to.not.be.null
        })
    })

    describe('invalid entries', () => {
        let page
        before(async () => {
            page = await browser.newPage();
            await page.goto(registrationURL);
            await page.waitForSelector('main h1');
        })
        after(async () => {
            await page.close()
        })

        beforeEach(async () => {
            await page.reload()
            await page.waitForSelector('main h1');
        })

        it('rejects submission with empty values', async () => {
            await page.click('main button')
            let error = await page.$('.v-alert.error')
            expect(error).to.not.be.null
        })

        it('rejects submission with invalid email', async () => {
            await fillReg(page)
            await page.evaluate(() => {
                document.getElementById('email').value = 'invalid'
            })
            await page.click('main button')
            let error = await page.$('.v-alert.error')
            expect(error).to.not.be.null
        })

        it('rejects submission with passwords that do not match', async () => {
            await fillReg(page)
            await page.evaluate(() => {
                document.getElementById('password').value = '123456'
                document.getElementById('confirm').value = '12345'
            })
            await page.click('main button')
            let error = await page.$('.v-alert.error')
            expect(error).to.not.be.null
        })

        it('rejects submission with short password', async () => {
            await fillReg(page)
            await page.evaluate(() => {
                document.getElementById('password').value = '123456'
                document.getElementById('confirm').value = '123456'
            })
            await page.click('main button')
            let error = await page.$('.v-alert.error')
            expect(error).to.not.be.null
        })

        it('rejects submission with cracked password', async () => {
            await fillReg(page)
            await page.evaluate(() => {
                document.getElementById('password').value = 'adminpassword'
                document.getElementById('confirm').value = 'adminpassword'
            })
            await page.click('main button')
            let error = await page.$('.v-alert.error')
            expect(error).to.not.be.null
        })
    })

    describe('success', () => {
        let page
        before(async () => {
            page = await browser.newPage();
            await page.goto(registrationURL);
            await page.waitForSelector('main h1');
        })
        after(async () => {
            await page.close()
        })

        it('rejects submission with invalid email', async () => {
            await fillReg(page)
            await Promise.all([
                page.waitForNavigation(),
                page.click('main button')
            ])
            let dashboard = await page.$('main h1')
            expect(dashboard).to.not.be.null
            expect(await (await dashboard.getProperty('textContent')).jsonValue()).to.equal('This is the Portal')
        })
    })
})
