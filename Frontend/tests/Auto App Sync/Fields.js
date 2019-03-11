const puppeteer = require ('puppeteer');

function timeout(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
};

function RegistrationBlank(){

  (async () => {

    let imgPath = 'Registration';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/add');
    await page.screenshot({path: (imgPath + '01.png')});
  
    // Try to register with a blank title.
    await page.click('.button-register')
    await page.screenshot({path: (imgPath + '02.png')});

    // Try to register with a blank launch url
    await page.type('#title', 'My Academic Pyramid');
    await page.click('.button-register');
    await page.screenshot({path: (imgPath + '03.png')});

    // Try to register with a blank email
    await page.type('#launchUrl', 'https://myacademicpyramid.com');
    await page.click('.button-register');
    await page.screenshot({path: (imgPath + '04.png')});

    // Try to register with a blank user deletion url
    await page.type('#email', 'krystalleon10@gmail.com');
    await page.click('.button-register');
    await page.screenshot({path: (imgPath + '05.png')});

    // Try to register with all valid fields
    //await page.type('#deleteUrl', 'https://myacademicpyramid.com/delete');
    //await page.$eval('button-register', form => form.click());
    //const element = document.getElementsByClassName('.form-register');
    //element.submit();
    //await page.screenshot({path: (imgPath + '06.png')});

    await browser.close()
  })();
}

function RegistrationInvalidEmail(){

  (async () => {

    let imgPath = 'Registration07.png';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/add');

    // Input title
    await page.type('#title', 'My Academic Pyramid');

    // Input launch url
    await page.type('#launchUrl', 'https://myacademicpyramid.com');

    // Input invalid email
    await page.type('#email', 'email');    

    // Input user deletion url
    await page.type('#deleteUrl', 'https://myacademicpyramid.com/delete');
    await page.click('.button-register');

    await page.screenshot({path: imgPath});

    await browser.close()
  })();
}

function DeletionBlank(){

  (async () => {

    let imgPath = 'Deletion';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/delete');
    await page.screenshot({path: (imgPath + '01.png')});
  
    // Try to register with a blank title.
    await page.click('.button-delete')
    await page.screenshot({path: (imgPath + '02.png')});

    // Try to register with a blank email
    await page.type('#title', 'My Academic Pyramid');
    await page.click('.button-delete');
    await page.screenshot({path: (imgPath + '03.png')});

    await browser.close()
  })();
}

function DeletionInvalidEmail(){

  (async () => {

    let imgPath = 'Deletion';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/delete');
  
    // Input title
    await page.type('#title', 'My Academic Pyramid');
    await page.click('.button-delete')

    // Input invalid email
    await page.type('#email', 'email');
    await page.click('.button-delete');
    await page.screenshot({path: (imgPath + '04.png')});

    await browser.close()
  })();
}

function GenerateKeyBlank(){

  (async () => {

    let imgPath = 'GenerateKey';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/key');
    await page.screenshot({path: (imgPath + '01.png')});
  
    // Try to register with a blank title.
    await page.click('.button-generate')
    await page.screenshot({path: (imgPath + '02.png')});

    // Try to register with a blank email
    await page.type('#title', 'My Academic Pyramid');
    await page.click('.button-generate');
    await page.screenshot({path: (imgPath + '03.png')});

    await browser.close()
  })();
}

function GenerateKeyInvalidEmail(){

  (async () => {

    let imgPath = 'GenerateKey';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/key');
  
    // Input title
    await page.type('#title', 'My Academic Pyramid');
    await page.click('.button-generate')

    // Input invalid email
    await page.type('#email', 'email');
    await page.click('.button-generate');
    await page.screenshot({path: (imgPath + '04.png')});

    await browser.close()
  })();
}

RegistrationBlank()
RegistrationInvalidEmail()
DeletionBlank()
DeletionInvalidEmail()
GenerateKeyBlank()
GenerateKeyInvalidEmail()