var swiper = new Swiper(".mySwiper", {
  spaceBetween: 30,
  centeredSlides: true,
  autoplay: {
    delay: 2500,
    disableOnInteraction: false,
  },
  pagination: {
    el: ".swiper-pagination",
    clickable: true,
  },
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },
});

const loginClick = document.querySelector('.login-section')
const registerLink = document.querySelector('.Register-link')
const loginLink = document.querySelector('.Login-link')

loginLink.addEventListener('click', () => {
  loginClick.classList.add('active')
})

registerLink.addEventListener('click', () => {
  loginClick.classList.remove('active')
})