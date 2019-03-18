const puppeteer = require ('puppeteer');

function timeout(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
};

// Register with all valid fields
function RegistrationValid(){

  (async () => {

    let imgPath = 'Registration_Valid_';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/add');
    await page.screenshot({path: (imgPath + '01.png')});

    // Input all fields
    await page.type('#title', 'My Application');
    await page.type('#launchUrl', 'https://myapplication.com');
    await page.type('#email', 'app@email.com');
    await page.type('#deleteUrl', 'https://myapplication.com/delete');
    await page.screenshot({path: (imgPath + '02.png')});

    // Click 'Register'
    await page.click('.button-register');
    await page.waitForSelector('#keyId');
    await page.screenshot({path: (imgPath + '03.png')});

    await browser.close()
  })();
}

// Attempt to register with blank fields
function RegistrationBlank(){

  (async () => {

    let imgPath = 'Registration_BlankFields_';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/add');
  
    // Try to register with a blank title.
    await page.click('.button-register')
    await page.screenshot({path: (imgPath + '01.png')});

    // Try to register with a blank launch url
    await page.type('#title', 'My Application');
    await page.click('.button-register');
    await page.screenshot({path: (imgPath + '02.png')});

    // Try to register with a blank email
    await page.type('#launchUrl', 'https://myapplication.com');
    await page.click('.button-register');
    await page.screenshot({path: (imgPath + '03.png')});

    // Try to register with a blank user deletion url
    await page.type('#email', 'app@email.com');
    await page.click('.button-register');
    await page.screenshot({path: (imgPath + '04.png')});

    await browser.close()
  })();
}

// Attempt to register with an invalid email
function RegistrationInvalidEmail(){

  (async () => {

    let imgPath = 'Registration_InvalidEmail.png';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/add');

    // Input title
    await page.type('#title', 'My Application');

    // Input launch url
    await page.type('#launchUrl', 'https://myapplication.com');

    // Input invalid email
    await page.type('#email', 'email');    

    // Input user deletion url
    await page.type('#deleteUrl', 'https://myapplication.com/delete');
    await page.click('.button-register');

    await page.screenshot({path: imgPath});

    await browser.close()
  })();
}

// Generate a key with valid inputs
function GenerateKeyValid(){

  (async () => {

    let imgPath = 'GenerateKey_Valid_';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/key');
    await page.screenshot({path: (imgPath + '01.png')});
  
    // Input fields
    await page.type('#title', 'My Application');
    await page.type('#email', 'app@email.com');
    await page.screenshot({path: (imgPath + '02.png')});

    await page.click('.button-generate');
    await page.waitForSelector('#generateId');
    await page.screenshot({path: (imgPath + '03.png')});

    await browser.close()
  })();
}

// Attempt to generate a key with blank fields
function GenerateKeyBlank(){

  (async () => {

    let imgPath = 'GenerateKey_BlankFields_';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/key');
  
    // Try to register with a blank title.
    await page.click('.button-generate')
    await page.screenshot({path: (imgPath + '01.png')});

    // Try to register with a blank email
    await page.type('#title', 'My Application');
    await page.click('.button-generate');
    await page.screenshot({path: (imgPath + '02.png')});

    await browser.close()
  })();
}

// Attempt to generate a key with an invalid email
function GenerateKeyInvalidEmail(){

  (async () => {

    let imgPath = 'GenerateKey_InvalidEmail.png';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/key');
  
    // Input title
    await page.type('#title', 'My Application');
    await page.click('.button-generate')

    // Input invalid email
    await page.type('#email', 'email');
    await page.click('.button-generate');
    await page.screenshot({path: (imgPath)});

    await browser.close()
  })();
}

// Delete an application
function DeletionValid(){

  (async () => {

    let imgPath = 'Deletion_Valid_';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/delete');
    await page.screenshot({path: (imgPath + '01.png')});

    // Input fields
    await page.type('#title', 'My Application');
    await page.type('#email', 'app@email.com');
    await page.screenshot({path: (imgPath + '02.png')});
    
    await page.click('.button-delete');
    await page.waitForSelector('#deleteId');
    await page.screenshot({path: (imgPath + '03.png')});

    await browser.close()
  })();
}

// Attempt to delete an application with blank fields
function DeletionBlank(){

  (async () => {

    let imgPath = 'Deletion_BlankFields_';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/delete');
  
    // Try to register with a blank title.
    await page.click('.button-delete')
    await page.screenshot({path: (imgPath + '01.png')});

    // Try to register with a blank email
    await page.type('#title', 'My Application');
    await page.click('.button-delete');
    await page.screenshot({path: (imgPath + '02.png')});

    await browser.close()
  })();
}

// Attempt to delete an application with an invalid email
function DeletionInvalidEmail(){

  (async () => {

    let imgPath = 'Deletion_InvalidEmail.png';

    // Open browser and navigate to app registration page
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('http://localhost:8080/#/delete');
  
    // Input title
    await page.type('#title', 'My Application');
    await page.click('.button-delete')

    // Input invalid email
    await page.type('#email', 'email');
    await page.click('.button-delete');
    await page.screenshot({path: (imgPath)});

    await browser.close()
  })();
}

RegistrationValid()
RegistrationBlank()
RegistrationInvalidEmail()
GenerateKeyValid()
GenerateKeyBlank()
GenerateKeyInvalidEmail()
DeletionBlank()
DeletionInvalidEmail()
DeletionValid()