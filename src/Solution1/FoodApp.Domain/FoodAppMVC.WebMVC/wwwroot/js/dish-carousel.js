document.addEventListener("DOMContentLoaded", function () {
    const track = document.querySelector(".carousel-track");
    const slides = Array.from(document.querySelectorAll(".carousel-slide"));
    const dotsContainer = document.querySelector(".carousel-dots");
    const dots = Array.from(dotsContainer?.children || []);

    let currentIndex = 0;
    const slideWidth = slides[0]?.offsetWidth + 16 || 300;

    function updateCarousel() {
        const offset = currentIndex * slideWidth;
        track.style.transform = `translateX(-${offset}px)`;
        dots.forEach((dot, i) => dot.classList.toggle("active", i === currentIndex));
    }

    function nextSlide() {
        currentIndex = (currentIndex + 1) % slides.length;
        updateCarousel();
    }

    if (track && slides.length > 0) {
        updateCarousel();
        setInterval(nextSlide, 5000);

        dots.forEach((dot, i) => {
            dot.addEventListener("click", () => {
                currentIndex = i;
                updateCarousel();
            });
        });
    }
});


