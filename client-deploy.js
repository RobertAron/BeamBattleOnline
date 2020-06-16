const ghPages = require('gh-pages')

ghPages.publish('Builds/Web/Client',(err)=>{
    console.log(err)
})