const puppeteer = require ('puppeteer');
const sso = 'https://kfc-sso.com/#';
const appTitle = 'My Application';
const appEmail = 'app@email.com';
const appLaunchUrl = 'https://myapplication.com';
const appDeleteUrl = 'https://myapplication.com/delete';

function timeout(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
};

// Register with all valid fields
async function RegistrationValid() {

  let imgPath = 'Registration_Valid_';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/add');
  await page.screenshot({path: (imgPath + '01.png')});

  // Input all fields
  await page.type('#title', appTitle);
  await page.type('#launchUrl', appLaunchUrl);
  await page.type('#email', appEmail);
  await page.type('#deleteUrl', appDeleteUrl);
  await page.screenshot({path: (imgPath + '02.png')});

  // Click 'Register'
  await page.click('button');
  await page.waitForSelector('h3');
  await page.screenshot({path: (imgPath + '03.png')});

  await browser.close()
};

// Register an existing application
async function RegistrationInvalidApp() {

  let imgPath = 'Registration_InvalidApp.png';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/add');

  // Input all fields
  await page.type('#title', appTitle);
  await page.type('#launchUrl', appLaunchUrl);
  await page.type('#email', appEmail);
  await page.type('#deleteUrl', appDeleteUrl);

  // Click 'Register'
  await page.click('button');
  await timeout(1000);
  await page.screenshot({path: (imgPath)});

  await browser.close()
};

// Attempt to register with blank fields
async function RegistrationBlank() {

  let imgPath = 'Registration_BlankFields_';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/add');

  // Try to register with a blank title.
  await page.click('button')
  await page.screenshot({path: (imgPath + '01.png')});

  // Try to register with a blank launch url
  await page.type('#title', appTitle);
  await page.click('button');
  await page.screenshot({path: (imgPath + '02.png')});

  // Try to register with a blank email
  await page.type('#launchUrl', appLaunchUrl);
  await page.click('button');
  await page.screenshot({path: (imgPath + '03.png')});

  // Try to register with a blank user deletion url
  await page.type('#email', appEmail);
  await page.click('button');
  await page.screenshot({path: (imgPath + '04.png')});

  await browser.close()
};

// Attempt to register with an invalid email
async function RegistrationInvalidEmail () {

  let imgPath = 'Registration_InvalidEmail.png';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/add');

  // Input title
  await page.type('#title', appTitle);

  // Input launch url
  await page.type('#launchUrl', appLaunchUrl);

  // Input invalid email
  await page.type('#email', 'email');    

  // Input user deletion url
  await page.type('#deleteUrl', appDeleteUrl);
  await page.click('button');
  await timeout(1000);

  await page.screenshot({path: imgPath});

  await browser.close()
};

// Attempt to register with an invalid launch url
async function RegistrationInvalidLaunchUrl () {

  let imgPath = 'Registration_InvalidLaunchUrl.png';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/add');

  // Input title
  await page.type('#title', appTitle);

  // Input launch url
  await page.type('#launchUrl', 'app url');

  // Input invalid email
  await page.type('#email', appEmail);    

  // Input user deletion url
  await page.type('#deleteUrl', appDeleteUrl);
  await page.click('button');
  await timeout(1000);

  await page.screenshot({path: imgPath});

  await browser.close()
};

// Attempt to register with an invalid user delete url
async function RegistrationInvalidDeleteUrl () {

  let imgPath = 'Registration_InvalidDeleteUrl.png';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/add');

  // Input title
  await page.type('#title', appTitle);

  // Input launch url
  await page.type('#launchUrl', appLaunchUrl);

  // Input invalid email
  await page.type('#email', appEmail);    

  // Input user deletion url
  await page.type('#deleteUrl', 'app url');
  await page.click('button');
  await timeout(1000);

  await page.screenshot({path: imgPath});

  await browser.close()
};

// Generate a key with valid inputs
async function GenerateKeyValid() {

  let imgPath = 'GenerateKey_Valid_';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/key');
  await page.screenshot({path: (imgPath + '01.png')});

  // Input fields
  await page.type('#title', appTitle);
  await page.type('#email', appEmail);
  await page.screenshot({path: (imgPath + '02.png')});

  await page.click('button');
  await page.waitForSelector('#hide');
  await page.screenshot({path: (imgPath + '03.png')});

  await browser.close()
};

// Generate a key with invalid inputs
async function GenerateKeyInvalidApp() {

  let imgPath = 'GenerateKey_InvalidApp.png';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/key');

  // Input fields
  await page.type('#title', 'my app');
  await page.type('#email', appEmail);

  await page.click('button');
  await timeout(1000);
  await page.screenshot({path: (imgPath)});

  await browser.close()
};

// Attempt to generate a key with blank fields
async function GenerateKeyBlank() {

  let imgPath = 'GenerateKey_BlankFields_';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/key');

  // Try to register with a blank title.
  await page.click('button')
  await page.screenshot({path: (imgPath + '01.png')});

  // Try to register with a blank email
  await page.type('#title', appTitle);
  await page.click('button');
  await page.screenshot({path: (imgPath + '02.png')});

  await browser.close()
};

// Attempt to generate a key with an invalid email
async function GenerateKeyInvalidEmail() {

  let imgPath = 'GenerateKey_InvalidEmail.png';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/key');

  // Input title
  await page.type('#title', appTitle);
  await page.click('button')

  // Input invalid email
  await page.type('#email', 'email');
  await page.click('button');
  await timeout(1000);
  await page.screenshot({path: (imgPath)});

  await browser.close()
};

// Delete an application
async function DeletionValid () {

  let imgPath = 'Deletion_Valid_';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/delete');
  await page.screenshot({path: (imgPath + '01.png')});

  // Input fields
  await page.type('#title', appTitle);
  await page.type('#email', appEmail);
  await page.screenshot({path: (imgPath + '02.png')});
  
  await page.click('button');
  await page.waitForSelector('#hide');
  await page.screenshot({path: (imgPath + '03.png')});

  await browser.close()
};

// Delete a non-existing application
async function DeletionInvalidApp () {

  let imgPath = 'Deletion_InvalidApp.png';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/delete');

  // Input fields
  await page.type('#title', 'my app');
  await page.type('#email', appEmail);
  
  await page.click('button');
  await timeout(1000);
  await page.screenshot({path: (imgPath)});

  await browser.close()
};

// Attempt to delete an application with blank fields
async function DeletionBlank() {

  let imgPath = 'Deletion_BlankFields_';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/delete');

  // Try to register with a blank title.
  await page.click('button')
  await page.screenshot({path: (imgPath + '01.png')});

  // Try to register with a blank email
  await page.type('#title', appTitle);
  await page.click('button');
  await page.screenshot({path: (imgPath + '02.png')});

  await browser.close()
};

// Attempt to delete an application with an invalid email
async function DeletionInvalidEmail() {

  let imgPath = 'Deletion_InvalidEmail.png';

  // Open browser and navigate to app registration page
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  await page.goto(sso + '/delete');

  // Input title
  await page.type('#title', appTitle);
  await page.click('button')

  // Input invalid email
  await page.type('#email', 'email');
  await page.click('button');
  await timeout(1000);
  await page.screenshot({path: (imgPath)});

  await browser.close()
};

async function Run(){
  await RegistrationValid();
  await RegistrationInvalidApp();
  await GenerateKeyValid();
  await DeletionValid();
  RegistrationBlank();
  RegistrationInvalidEmail();
  RegistrationInvalidLaunchUrl();
  RegistrationInvalidDeleteUrl();
  GenerateKeyBlank();
  GenerateKeyInvalidEmail();
  GenerateKeyInvalidApp();
  DeletionBlank();
  DeletionInvalidEmail();
  DeletionInvalidApp();
}

Run();