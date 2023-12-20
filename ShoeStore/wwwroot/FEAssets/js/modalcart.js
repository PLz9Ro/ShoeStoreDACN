document.addEventListener(
  "click",
  function (event) {
    if (event.target.matches(".fsb-close")) {
      event.preventDefault();
      closeitall();
      return;
    }
    if (event.target.hasAttribute("data-fsc-addthis")) {
      var product = event.target.getAttribute("data-fsc-addthis");
      var cart = event.target.getAttribute("data-fsc-cart");
      fsaddProd(product, cart);
      return;
    }
    if (event.target.hasAttribute("data-fsc-opencart")) {
      openCart(event.target.getAttribute("data-fsc-opencart"));
      return;
    }
    return;
  },
  false
);

function fsaddProd(prod, type) {
  fastspring.builder.add(prod, function (data) {
    if (
      type != "fsb-MOD" &&
      type != "fsb-MODSM" &&
      type != "fsb-RS" &&
      type != "fsb-LS" &&
      type != "fsb-BS" &&
      type != "Modal" &&
      type != "SmallModal" &&
      type != "Right" &&
      type != "Left" &&
      type != "Bottom"
    ) {
      openCart("fsb-MOD");
    } else {
      openCart(type);
    }
  });
}

function applycoupon() {
  var couponid = document.getElementById("couponcode").value;
  fastspring.builder.promo(couponid);
}

function markupHelpersCallback() {
  Handlebars.registerHelper(
    "iff",
    function (lvalue, operator, rvalue, options) {
      var functions = {
        "==": function (l, r) {
          return l == r;
        },
        "===": function (l, r) {
          return l === r;
        },
        "!=": function (l, r) {
          return l != r;
        },
        "<": function (l, r) {
          return l < r;
        },
        ">": function (l, r) {
          return l > r;
        },
        "<=": function (l, r) {
          return l <= r;
        },
        ">=": function (l, r) {
          return l >= r;
        },
        typeof: function (l, r) {
          return typeof l === r;
        },
      };
      if (!functions.hasOwnProperty(operator)) {
        throw new Error(
          "Handlerbars Helper 'iff' doesn't know the operator " + operator
        );
      }
      var result = functions[operator](lvalue, rvalue);
      if (result) {
        return options.fn(this);
      } else {
        return options.inverse(this);
      }
    }
  );
  Handlebars.registerPartial(
    "pricing",
    document.getElementById("pricing-partial").innerHTML
  );
  Handlebars.registerPartial(
    "quantity",
    document.getElementById("quantity-partial").innerHTML
  );
  Handlebars.registerPartial(
    "pricing2",
    document.getElementById("pricing-partial2").innerHTML
  );
  Handlebars.registerPartial(
    "volume-discount",
    document.getElementById("volume-discount").innerHTML
  );
  Handlebars.registerPartial(
    "xsell",
    document.getElementById("xsell").innerHTML
  );
  Handlebars.registerPartial(
    "upsell",
    document.getElementById("upsell").innerHTML
  );
  Handlebars.registerPartial(
    "singlechoice",
    document.getElementById("singlechoice").innerHTML
  );
  Handlebars.registerPartial(
    "multichoice",
    document.getElementById("multichoice").innerHTML
  );
}

function beforeRequestsCallbackFunction() {
  var fsb_spinner = document.getElementById("fastspring_spinner");
  fsb_spinner.style.display = "block";
}

function afterMarkupCallbackFunction() {
  init();
  var fsb_spinner = document.getElementById("fastspring_spinner");
  fsb_spinner.style.animationName = "fsb-revfadeIn";
  setTimeout(function () {
    fsb_spinner.style.animationName = "fsb-fadeIn";
    fsb_spinner.style.display = "none";
  }, 450);
}

function decorateCallback(url) {
  var linkerParam = null;
  if (ga && typeof ga === "function") {
    ga(function () {
      var trackers = ga.getAll();
      linkerParam = trackers[0].get("linkerParam");
    });
  }
  return linkerParam ? url + "?" + linkerParam : url;
}

function errorCallback(code, string) {
  var fsb_spinner = document.getElementById("fastspring_spinner");
  fsb_spinner.style.animationName = "fsb-revfadeIn";
  setTimeout(function () {
    fsb_spinner.style.animationName = "fsb-fadeIn";
    fsb_spinner.style.display = "none";
  }, 450);
  var fsb_modal = document.getElementById("fsb-modal");
  if (fsb_modal.style.display === "block") {
    var fsb_error = document.getElementById("fsb_error");
    var fsb_error_msg = document.getElementById("fsb_error_msg");
    fsb_error.style.display = "block";
    fsb_error.style.animationName = "fsb-mod_animatetop";
    fsb_error_msg.innerHTML = "Error: " + code + " - " + string;
    setTimeout(function () {
      fsb_error.style.animationName = "fsb-mod_revanimatetop";
      setTimeout(function () {
        fsb_error.style.display = "none";
      }, 400);
    }, 5000);
  } else {
    var fsb_error = document.getElementById("fsb_error");
    var fsb_error_msg = document.getElementById("fsb_error_msg");
    fsb_error.style.display = "block";
    fsb_error.style.animationName = "fsb-mod_animatetop";
    fsb_error_msg.innerHTML = "Error: " + code + " - " + string;
    setTimeout(function () {
      fsb_error.style.animationName = "fsb-mod_revanimatetop";
      setTimeout(function () {
        fsb_error.style.display = "none";
      }, 400);
    }, 5000);
  }
}

function dataCallback(data) {
  if (data.messages[0]) {
    var fsb_modal = document.getElementById("fsb-modal");
    if (fsb_modal.style.display === "block") {
      var fsb_error = document.getElementById("fsb_error");
      var fsb_error_msg = document.getElementById("fsb_error_msg");
      fsb_error.style.display = "block";
      fsb_error.style.animationName = "fsb-mod_animatetop";
      fsb_error_msg.innerHTML = data.messages[0].phrase;
      setTimeout(function () {
        fsb_error.style.animationName = "fsb-mod_revanimatetop";
        setTimeout(function () {
          fsb_error.style.display = "none";
        }, 400);
      }, 5000);
    } else {
      var fsb_error = document.getElementById("fsb_error");
      var fsb_error_msg = document.getElementById("fsb_error_msg");
      fsb_error.style.display = "block";
      fsb_error.style.animationName = "fsb-mod_animatetop";
      fsb_error_msg.innerHTML = data.messages[0].phrase;
      setTimeout(function () {
        fsb_error.style.animationName = "fsb-mod_revanimatetop";
        setTimeout(function () {
          fsb_error.style.display = "none";
        }, 400);
      }, 5000);
    }
  }
  var minicart = document.getElementById("minicart-count");
  if (minicart) {
    let inCart = 0;
    if (data && data.hasOwnProperty("groups")) {
      const { groups } = data;
      groups.forEach((group) => {
        if (group.items && Array.isArray(group.items)) {
          group.items.forEach((item) => {
            if (item.selected) {
              inCart += item.quantity;
            }
          });
        }
      });
    }
    document.getElementById("minicart-count").innerHTML = inCart;
  }
}

var fsb_modal = document.getElementById("fsb-modal");
var fsb_modalcontent = document.querySelectorAll(
  "#fsb-modal .fsb-modal-content"
);

function openCart(type) {
  if (type == "Modal") {
    type = "fsb-MOD";
  }
  if (type == "ModalSmall") {
    type = "fsb-MODSM";
  }
  if (type == "Left") {
    type = "fsb-LS";
  }
  if (type == "Right") {
    type = "fsb-RS";
  }
  if (type == "Bottom") {
    type = "fsb-BS";
  }
  var fsb_modal = document.getElementById("fsb-modal");
  var fsb_modalcontent = document.querySelectorAll(
    "#fsb-modal .fsb-modal-content"
  );
  if (!type) {
    type = "fsb-MOD";
  }
  fsb_modal.classList.add(type);
  if (type == "fsb-LS") {
    fsb_modalcontent[0].style.animationName = "fsb-ls_slideIn";
  }
  if (type == "fsb-RS") {
    fsb_modalcontent[0].style.animationName = "fsb-rs_slideIn";
  }
  if (type == "fsb-BS") {
    fsb_modalcontent[0].style.animationName = "fsb-bs_slideIn";
  }
  if (type == "fsb-MOD" || type == "fsb-MODSM") {
    fsb_modalcontent[0].style.animationName = "fsb-mod_animatetop";
  }
  fsb_modal.style.display = "block";
  document.body.classList.add("fsb-modalOpen");
}

function closeitall() {
  var fsb_modal = document.getElementById("fsb-modal");
  var fsb_modalcontent = document.querySelectorAll(
    "#fsb-modal .fsb-modal-content"
  );
  document.body.classList.remove("fsb-modalOpen");
  if (fsb_modal.classList.contains("fsb-BS")) {
    fsb_modalcontent[0].style.animationName = "fsb-bs_revslideIn";
    fsb_modal.style.animationName = "fsb-revfadeIn";
    setTimeout(function () {
      fsb_modal.style.display = "none";
      fsb_modalcontent[0].style.animationName = "fsb-bs_slideIn";
      fsb_modal.style.animationName = "fsb-fadeIn";
    }, 350);
  }
  if (fsb_modal.classList.contains("fsb-LS")) {
    fsb_modalcontent[0].style.animationName = "fsb-ls_revslideIn";
    fsb_modal.style.animationName = "fsb-revfadeIn";
    setTimeout(function () {
      fsb_modal.style.display = "none";
      fsb_modalcontent[0].style.animationName = "fsb-ls_slideIn";
      fsb_modal.style.animationName = "fsb-fadeIn";
    }, 350);
  }
  if (fsb_modal.classList.contains("fsb-RS")) {
    fsb_modalcontent[0].style.animationName = "fsb-rs_revslideIn";
    fsb_modal.style.animationName = "fsb-revfadeIn";
    setTimeout(function () {
      fsb_modal.style.display = "none";
      fsb_modalcontent[0].style.animationName = "fsb-rs_slideIn";
      fsb_modal.style.animationName = "fsb-fadeIn";
    }, 350);
  }
  if (
    fsb_modal.classList.contains("fsb-MOD") ||
    fsb_modal.classList.contains("fsb-MODSM")
  ) {
    fsb_modalcontent[0].style.animationName = "fsb-mod_revanimatetop";
    fsb_modal.style.animationName = "fsb-revfadeIn";
    setTimeout(function () {
      fsb_modal.style.display = "none";
      fsb_modalcontent[0].style.animationName = "fsb-mod_animatetop";
      fsb_modal.style.animationName = "fsb-fadeIn";
    }, 350);
  }
  setTimeout(function () {
    document.getElementById("fsb-modal").classList.remove("fsb-RS");
    document.getElementById("fsb-modal").classList.remove("fsb-LS");
    document.getElementById("fsb-modal").classList.remove("fsb-BS");
    document.getElementById("fsb-modal").classList.remove("fsb-MOD");
    document.getElementById("fsb-modal").classList.remove("fsb-MODSM");
  }, 350);
}

window.onclick = function (event) {
  if (event.target.id == "fsb-modal") {
    closeitall();
  }
};

var value,
  quantity = document.getElementsByClassName("fsb-number");
function createBindings(quantityContainer) {
  var quantityAmount =
    quantityContainer.getElementsByClassName("fsb-qtyinput")[0];
  var increase = quantityContainer.getElementsByClassName("fsb-plus")[0];
  var decrease = quantityContainer.getElementsByClassName("fsb-minus")[0];
  increase.addEventListener("click", function () {
    increaseValue(this, quantityAmount);
  });
  decrease.addEventListener("click", function () {
    decreaseValue(this, quantityAmount);
  });
}

function init() {
  for (var i = 0; i < quantity.length; i++) {
    createBindings(quantity[i]);
  }
}

function increaseValue(clicker, quantityAmount) {
  value = parseInt(quantityAmount.value, 10);
  product = quantityAmount.getAttribute("data-fsc-item-path");
  value = isNaN(value) ? 0 : value;
  value++;
  quantityAmount.value = value;
  fastspring.builder.update(product, value);
}

function decreaseValue(clicker, quantityAmount) {
  value = parseInt(quantityAmount.value, 10);
  product = quantityAmount.getAttribute("data-fsc-item-path");
  value = isNaN(value) ? 0 : value;
  if (value > 0) value--;
  quantityAmount.value = value;
  fastspring.builder.update(product, value);
}
